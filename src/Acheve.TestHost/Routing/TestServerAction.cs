using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            var (fromType, canBeObjectWithMultipleFroms, isNeverBind) = GetIsFrom(argument, activeBodyApiController, value => value.ParameterType, value => value.GetCustomAttributes());

            if (!ArgumentValues.ContainsKey(order))
            {
                if (IsNullable(argument.ParameterType))
                {
                    var expressionValue = Expression.Lambda(expression).Compile().DynamicInvoke();

                    if (expressionValue != null)
                    {
                        ArgumentValues.Add(order, new TestServerArgument(expressionValue.ToString(), fromType, isNeverBind, argument.ParameterType, argument.Name));
                    }
                }
                else
                {
                    switch (expression)
                    {
                        case ConstantExpression constant:
                            {
                                ArgumentValues.Add(order, new TestServerArgument(constant.Value?.ToString(), fromType, isNeverBind, argument.ParameterType, argument.Name));
                            }
                            break;

                        case MemberExpression member when member.NodeType == ExpressionType.MemberAccess:
                            {
                                var instance = Expression.Lambda(member)
                                    .Compile()
                                    .DynamicInvoke();
                                AddArgumentValues(order, instance, argument.Name, fromType, isNeverBind, argument.ParameterType, canBeObjectWithMultipleFroms);
                            }
                            break;

                        case MethodCallExpression method:
                            {
                                var instance = Expression.Lambda(method).Compile().DynamicInvoke();
                                AddArgumentValues(order, instance, argument.Name, fromType, isNeverBind, argument.ParameterType, canBeObjectWithMultipleFroms);
                            }
                            break;

                        default: return;
                    }
                }
            }
        }

        private static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

        private void AddArgumentValues(int order, object value, string argumentName,
            TestServerArgumentFromType fromType, bool isNeverBind, Type type, bool canBeObjectWithMultipleFroms)
        {
            if (canBeObjectWithMultipleFroms)
            {
                var properties = value.GetType().GetProperties();
                var isObjectWithMultipleFroms = properties
                    .SelectMany(p => p.GetCustomAttributes())
                    .Select(a => a.GetType())
                    .Any(a => a == typeof(FromBodyAttribute) || a == typeof(FromRouteAttribute) || a == typeof(FromHeaderAttribute) || a == typeof(FromQueryAttribute));
                if (isObjectWithMultipleFroms)
                {
                    foreach (var property in properties)
                    {
                        (fromType, canBeObjectWithMultipleFroms, var isNeverBindProp) = GetIsFrom(property, false, value => value.PropertyType, value => value.GetCustomAttributes());
                        argumentName = property.Name;
                        var propertyValue = property.GetValue(value);

                        ArgumentValues.Add(order, new TestServerArgument(propertyValue, fromType, isNeverBind || isNeverBindProp, property.PropertyType, argumentName));
                        order++;
                    }

                    return;
                }
            }

            ArgumentValues.Add(order, new TestServerArgument(value, fromType, isNeverBind, type, argumentName));
        }

        private (TestServerArgumentFromType fromType, bool canBeObjectWithMultipleFroms, bool neverBind) GetIsFrom<T>(T value, bool activeBodyApiController, Func<T, Type> getTypeFunc, Func<T, IEnumerable<Attribute>> getAttributesFunc)
        {
            var fromType = TestServerArgumentFromType.None;
            var type = getTypeFunc(value);
            var attributes = getAttributesFunc(value);

            var isFromBody = attributes.Any(a => a is FromBodyAttribute);
            var isFromForm = attributes.Any(a => a is FromFormAttribute);
            var isFromHeader = attributes.Any(a => a is FromHeaderAttribute);
            var isFromRoute = attributes.Any(a => a is FromRouteAttribute);
            var isFromQuery = attributes.Any(a => a is FromQueryAttribute);
            var isBindNever = attributes.Any(a => a is BindNeverAttribute);

            if (isFromBody)
            {
                fromType |= TestServerArgumentFromType.Body;
            }
            if (isFromForm)
            {
                fromType |= TestServerArgumentFromType.Form;
            }
            if (isFromHeader)
            {
                fromType |= TestServerArgumentFromType.Header;
            }
            if (isFromRoute)
            {
                fromType |= TestServerArgumentFromType.Route;
            }
            if (isFromQuery)
            {
                fromType |= TestServerArgumentFromType.Query;
            }

            bool isPrimitive = type.IsPrimitiveType();
            bool hasNoAttributes = fromType == TestServerArgumentFromType.None;
            bool canBeObjectWithMultipleFroms = false;
            if (hasNoAttributes && !isPrimitive)
            {
#if NET8_0_OR_GREATER
                canBeObjectWithMultipleFroms = MethodInfo.GetParameters().Length == 1;
#else
                canBeObjectWithMultipleFroms = false;
#endif

                if (activeBodyApiController)
                {
                    fromType = TestServerArgumentFromType.Body;
                }
            }

            if (type == typeof(System.Threading.CancellationToken))
            {
                fromType = TestServerArgumentFromType.None;
            }

            if (type == typeof(IFormFile))
            {
                fromType = TestServerArgumentFromType.Form;
            }

            return (fromType, canBeObjectWithMultipleFroms, isBindNever);
        }
    }
}