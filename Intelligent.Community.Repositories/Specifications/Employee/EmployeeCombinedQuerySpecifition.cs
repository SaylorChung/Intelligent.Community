using Intelligent.Community.DataObjects;
using Intelligent.Community.Domain.Enums;
using Intelligent.Community.Domain.Models;
using Saylor.Specifications;
using System;
using System.Linq.Expressions;

namespace Intelligent.Community.Domain.Repositories.Specifications
{
    /// <summary>
    ///     组合查询规约。
    /// </summary>
    public class EmployeeCombinedQuerySpecifition : Specification<Employee>
    {
        private readonly EmployeeDataObject _employee;

        public EmployeeCombinedQuerySpecifition(EmployeeDataObject employee)
        {
            this._employee = employee;
        }

        public override System.Linq.Expressions.Expression<Func<Employee, bool>> GetExpression()
        {
            Expression<Func<Employee, bool>> exp = f => true;

            if (!string.IsNullOrEmpty(_employee.UserName))
                exp = exp.And(m => m.UserName.Contains(_employee.UserName));
            if (!string.IsNullOrEmpty(_employee.RealName))
                exp = exp.And(m => m.RealName.Contains(_employee.RealName));
            if (!string.IsNullOrEmpty(_employee.Mobile))
                exp = exp.And(m => m.Mobile.Contains(_employee.Mobile));
            if (Enum.IsDefined(typeof(StatusDataObject), _employee.Status))
                exp = exp.And(m => m.Status == (Status)_employee.Status);

            return exp;
        }
    }   
}
