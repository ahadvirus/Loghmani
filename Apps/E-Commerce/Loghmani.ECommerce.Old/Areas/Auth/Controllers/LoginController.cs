using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Loghmani.ECommerce.Old.Areas.Auth.Models.DataTransfers.Login;
using Loghmani.ECommerce.Old.Areas.Auth.Models.Profiles.Login;
using Loghmani.ECommerce.Old.Areas.Auth.Models.ViewModels.Login;
using Loghmani.ECommerce.Old.Data;
using Loghmani.ECommerce.Old.Infrastructures.Configurations;
using Loghmani.ECommerce.Old.Infrastructures.Extensions;
using Loghmani.ECommerce.Old.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using SqlKata;

namespace Loghmani.ECommerce.Old.Areas.Auth.Controllers;

[Area(nameof(Area.Auth))]
public class LoginController : Controller
{
    private IMapper Mapper { get; }
    private DatabaseContext Context { get; }
    private IStringLocalizer<LoginController> Localizer { get; }

    public LoginController(DatabaseContext context, IStringLocalizer<LoginController> localizer)
    {
        Context = context;
        Localizer = localizer;

        Mapper = new Mapper(
            new MapperConfiguration(
                config =>
            config.AddProfiles(new Profile[]
                {
                    new RequestToLoginProfile()
                })
            )
        );
    }

    [HttpGet]
    public IActionResult Index([Bind(nameof(RequestVM.ReturnUrl))] RequestVM request)
    {
        CultureInfo culture = Thread.CurrentThread.CurrentCulture;
        ViewData[nameof(Infrastructures.Configurations.View.Title)] = Localizer[name: "IndexTitle"];

        return View(model: Mapper.Map<RequestVM, LoginVM>(request));
    }

    [HttpPost, ActionName(name: nameof(Index))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IndexConfirmed(
        [Bind(nameof(LoginVM.ReturnUrl), nameof(LoginVM.ReturnUrl), nameof(LoginVM.ReturnUrl))] LoginVM entry)
    {
        IActionResult result = View(entry);

        if (ModelState.IsValid)
        {
            using (DbConnection connection = await Context.Connection())
            {
                // Checking Username and Password is invalid
                Query query = new Query(table: nameof(User))
                    .Select(nameof(Old.Models.Entities.User.Id))
                    .Where(column: nameof(Old.Models.Entities.User.Username), value: entry.Username)
                    .Where(column: nameof(Old.Models.Entities.User.Password), value: entry.Password);

                SqlResult sql = await Context.Compile(query);

                int id = await connection.QueryFirstOrDefaultAsync<int>(sql: sql.Sql, param: sql.Bindings);

                if (id != 0)
                {
                    // Fetching data of user founded from sql
                    query = new Query(nameof(Old.Models.Entities.User))
                        .Select(string.Format("{0}.{1}", nameof(Old.Models.Entities.User), nameof(Old.Models.Entities.User.Id)))
                        .Select(
                             callback: q => q.From(nameof(UserClaim))
                                .Select(nameof(UserClaim.Value))
                                .Where(
                                    column: nameof(UserClaim.UserId),
                                    value: id
                                )
                                .Where(
                                    column: nameof(UserClaim.Key),
                                    value: nameof(UserDTO.Name)
                                ),
                            alias: nameof(UserDTO.Name)
                        )
                        .Select(
                            callback: q => q.From(nameof(UserClaim))
                                .Select(nameof(UserClaim.Value))
                                .Where(
                                    column: nameof(UserClaim.UserId),
                                    value: id
                                )
                                .Where(
                                    column: nameof(UserClaim.Key),
                                    value: nameof(UserDTO.Family)
                                ),
                            alias: nameof(UserDTO.Family)
                            )
                        .Select(
                            callback: q => q.Select(string.Format("{0}.{1}", nameof(Role), nameof(Role.Id))),
                            alias: nameof(RoleUser.RoleId)
                        )
                        .Select(
                            callback: q => q.Select(string.Format("{0}.{1}", nameof(Role), nameof(Role.Name))),
                            alias: string.Format("{0}{1}", nameof(Role), nameof(Role.Name))
                        )
                        .Where(
                            column: string.Format("{0}.{1}", nameof(Old.Models.Entities.User), nameof(Old.Models.Entities.User.Id)),
                            value: id
                        )
                        .Join(
                            table: nameof(RoleUser),
                            first: string.Format("{0}.{1}", nameof(Old.Models.Entities.User), nameof(Old.Models.Entities.User.Id)),
                            second: string.Format("{0}.{1}", nameof(RoleUser), nameof(RoleUser.UserId))
                        )
                        .Join(
                            table: nameof(Role),
                            first: string.Format("{0}.{1}", nameof(RoleUser), nameof(RoleUser.RoleId)),
                            second: string.Format("{0}.{1}", nameof(Role), nameof(Role.Id))
                        );

                    sql = await Context.Compile(query);

                    UserDTO? user = null;

                    _ = await connection.QueryAsync<UserDTO, string, UserDTO>(
                        sql: sql.Sql,
                        map: (entity, role) =>
                        {
                            if (user == null)
                            {
                                user = entity;
                            }

                            user.Roles.Add(role);

                            return entity;
                        },
                        param: sql.Bindings,
                        splitOn: nameof(RoleUser.RoleId)
                    );

                    if (user != null)
                    {
                        result = Redirect(
                            string.IsNullOrEmpty(entry.ReturnUrl)
                            ? HttpContext.GetHostName()
                            : entry.ReturnUrl
                            );

                        await HttpContext.SignInAsync(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(
                                    new List<Claim>()
                                    {
                                        new Claim(type: ClaimTypes.NameIdentifier, value: user.Id.ToString()),
                                        new Claim(type: ClaimTypes.Name, value: string.Format("{0} {1}", user.Name, user.Family)),
                                        new Claim(type: ClaimTypes.Role, value: string.Join(", ", user.Roles))
                                    })
                                ),
                            new AuthenticationProperties()
                            {
                                ExpiresUtc = DateTimeOffset.Now.AddDays(7)
                            });
                    }
                }
                else
                {
                    string errorKey = string.Format("{0}Or{1}", nameof(LoginVM.Username), nameof(LoginVM.Password));
                    ModelState.AddModelError(
                        key: errorKey,
                        errorMessage: Localizer[string.Format("ErrorMessage{0}IsInvalid", errorKey)]
                    );
                }
            }
        }

        return result;

    }
}