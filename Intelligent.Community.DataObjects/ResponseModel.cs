using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent.Community.DataObjects
{
    /// <summary>
    ///     响应对象。
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        ///     是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        ///     状态码
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        ///     相应结果
        /// </summary>
        public object Result { get; set; }
    }
}
