using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Acheve.TestHost.Routing
{
    public class TestServerAction
    {
        public MethodInfo MethodInfo { get; private set; }

        public Dictionary<int, TestServerArgument> ArgumentValues { get; private set; }


        public TestServerAction(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            ArgumentValues = new Dictionary<int, TestServerArgument>();
        }

        public void AddArgument(int order, Expression expression)
        {
            var argument = MethodInfo.GetParameters()[order];
            var isFromBody = argument.GetCustomAttributes<FromBodyAttribute>().Any();
            var isFromForm = argument.GetCustomAttributes<FromFormAttribute>().Any();

            if (!ArgumentValues.ContainsKey(order))
            {
                switch (expression)
                {
                    case ConstantExpression constant:
                        {
                            ArgumentValues.Add(order, new TestServerArgument(constant.Value.ToString(), isFromBody, isFromForm));
                        }
                        break;
                    case MemberExpression member when member.NodeType == ExpressionType.MemberAccess:
                        {
                            var instance = Expression.Lambda(member)
                                .Compile()
                                .DynamicInvoke();

                            ArgumentValues.Add(order, new TestServerArgument(instance, isFromBody, isFromForm));
                        }
                        break;
                    default: return;
                }
            }
        }
    }
}
