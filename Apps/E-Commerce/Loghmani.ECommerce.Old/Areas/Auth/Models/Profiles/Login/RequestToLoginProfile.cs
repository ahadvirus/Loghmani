using AutoMapper;
using Loghmani.ECommerce.Old.Areas.Auth.Models.ViewModels.Login;

namespace Loghmani.ECommerce.Old.Areas.Auth.Models.Profiles.Login;

public class RequestToLoginProfile : Profile
{
    public RequestToLoginProfile()
    {
        CreateMap<RequestVM, LoginVM>()
            .ForMember(des => des.ReturnUrl,
                opt => opt.MapFrom(
                    src => src.ReturnUrl
                ))
            .ForMember(des => des.Username,
                opt => opt.MapFrom(
                    src => string.Empty
                ))
            .ForMember(des => des.Password,
                opt => opt.MapFrom(
                    src => string.Empty
                ));

    }
}