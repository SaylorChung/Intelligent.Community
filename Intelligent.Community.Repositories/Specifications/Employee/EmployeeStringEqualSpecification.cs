using Intelligent.Community.Domain.Models;
using Saylor.Specifications;

namespace Intelligent.Community.Domain.Repositories.Specifications
{
    /// <summary>
    ///     标识“员工”领域模型的字符串Equal规约抽象基类。
    /// </summary>
    public abstract class EmployeeStringEqualSpecification : Specification<Employee>
    {
        protected readonly string _value;

        public EmployeeStringEqualSpecification(string value)
        {
            this._value = value;
        }
    }
}
