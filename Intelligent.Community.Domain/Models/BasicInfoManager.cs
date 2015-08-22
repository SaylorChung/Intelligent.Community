using Saylor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelligent.Community.Domain.Models
{
    /// <summary>
    ///     车辆信息
    /// </summary>
    public class Car:IEntity
    {
        /// <summary>
        ///     uniq-identifier
        /// </summary>
        public virtual Guid ID { get; set; }
        /// <summary>
        /// 车辆名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 车辆编号
        /// </summary>
        public virtual string CarNum { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public virtual string CarCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Memo { get; set; }
    }
}
