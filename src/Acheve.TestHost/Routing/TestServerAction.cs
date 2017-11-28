using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Acheve.TestHost.Routing
{
    public class TestServerAction
    {
        public MethodInfo MethodInfo { get; private set; }

        public Dictionary<int,object> ArgumentValues { get; private set; }


        public TestServerAction(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            ArgumentValues = new Dictionary<int, object>();
        }

        public void AddArgument(int order,Expression expression)
        {
            if (!ArgumentValues.ContainsKey(order))
            {
                switch(expression)
                {
                    case ConstantExpression constant:
                        {
                            ArgumentValues.Add(order, constant.Value.ToString());
                        }break;
                    case MemberExpression member when member.NodeType == ExpressionType.MemberAccess:
                        {
                            var instance = Expression.Lambda(member)
                                .Compile()
                                .DynamicInvoke();

                            ArgumentValues.Add(order, instance);
                        }
                        break;
                    default: return;
                }
                
            }
        }
    }
}
