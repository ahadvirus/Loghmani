using System;
using System.ComponentModel.DataAnnotations;

namespace Loghmani.ECommerce.Old.Infrastructures.Configurations
{
    public class DataAnnotations
    {
        public string Required
        {
            get
            {
                return nameof(RequiredAttribute).Replace(oldValue: nameof(Attribute), newValue: string.Empty);
            }
        }
    }
}
