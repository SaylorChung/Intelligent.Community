using Intelligent.Community.Domain.Models;
using System;

namespace Intelligent.Community.Domain.Repositories.Specifications
{
    /// <summary>
    ///     员工用户名比对规约。
    /// </summary>
    public class EmployeeUserNameEqualSpecification:EmployeeStringEqualSpecification
    {
        public EmployeeUserNameEqualSpecification(string userName)
            : base(userName) { }

        public override System.Linq.Expressions.Expression<Func<Employee, bool>> GetExpression()
        {
            return c => c.UserName == _value;
        }
    }
}
