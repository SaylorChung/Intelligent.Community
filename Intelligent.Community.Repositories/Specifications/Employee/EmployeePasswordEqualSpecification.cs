using Intelligent.Community.Domain.Models;
using System;

namespace Intelligent.Community.Domain.Repositories.Specifications
{
    /// <summary>
    ///     员工密码比对规约。
    /// </summary>
    public class EmployeePasswordEqualSpecification:EmployeeStringEqualSpecification
    {
        public EmployeePasswordEqualSpecification(string password)
            : base(password) { }

        public override System.Linq.Expressions.Expression<Func<Employee, bool>> GetExpression()
        {
            return c => c.Password == _value;
        }
    }
}
