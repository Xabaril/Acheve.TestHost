using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        public void AddArgument(int order, Expression expression, bool activeBodyApiController)
        {
            var argument = MethodInfo.GetParameters()[order];
            var isFromBody = argument.GetCustomAttributes<FromBodyAttribute>().Any();
            var isFromForm = argument.GetCustomAttributes<FromFormAttribute>().Any();
            var isFromHeader = argument.GetCustomAttributes<FromHeaderAttribute>().Any();
            var isFromQuery = argument.GetCustomAttributes<FromQueryAttribute>().Any();

            bool isPrimitive = argument.ParameterType.IsPrimitive || argument.ParameterType.Name.Equals(typeof(string));
            bool hasNoAttributes = !isFromBody && !isFromForm && !isFromHeader;

            if (activeBodyApiController && hasNoAttributes && !isPrimitive)
            {
                isFromBody = true;
            }

            bool isPrimitive = argument.ParameterType.IsPrimitive || argument.ParameterType.Name.Equals(typeof(string));
            bool hasNoAttributes = !isFromBody && !isFromForm && !isFromHeader;

            if (activeBodyApiController && hasNoAttributes && !isPrimitive)
            {
                isFromBody = true;
            }

            if (!ArgumentValues.ContainsKey(order))
            {
                switch (expression)
                {
                    case ConstantExpression constant:
                        {
                            ArgumentValues.Add(order, new TestServerArgument(constant.Value?.ToString(), isFromBody, isFromForm, isFromHeader, argument.Name));
                        }
                        break;
                    case MemberExpression member when member.NodeType == ExpressionType.MemberAccess:
                        {
                            var instance = Expression.Lambda(member)
                                .Compile()
                                .DynamicInvoke();

                            ArgumentValues.Add(order, new TestServerArgument(instance, isFromBody, isFromForm, isFromHeader, argument.Name));
                        }
                        break;
                    case MethodCallExpression method:
                        {
                            var instance = Expression.Lambda(method).Compile().DynamicInvoke();
                            ArgumentValues.Add(order, new TestServerArgument(instance, isFromBody, isFromForm, isFromHeader, argument.Name));
                        }
                        break;
                    default: return;
                }
            }
        }
    }
}
