using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityPaymentSystem.Application.Core
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Messages { get; set; }
        public T Datas { get; set; }

        public static ServiceResult<T> Ok(T data, string message = "") =>
            new ServiceResult<T> { Success = true, Datas = data, Messages = message };

        public static ServiceResult<T> Error(string message) =>
            new ServiceResult<T> { Success = false, Messages = message };
    }
}
