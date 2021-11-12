using System;
using System.Linq;
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

namespace Zack.Infrastructure.EFCore;
public class ExpressionHelper
{
    /// <summary>
    /// Users.SingleOrDefaultAsync(MakeEqual((User u) => u.PhoneNumber, phoneNumber))
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="propAccessor"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Expression<Func<TItem, bool>> MakeEqual<TItem, TProp>(Expression<Func<TItem, TProp>> propAccessor, TProp? other)
        where TItem : class
        where TProp : class
    {
        var e1 = propAccessor.Parameters.Single();//提取出来参数
        BinaryExpression? conditionalExpr = null;
        foreach (var prop in typeof(TProp).GetProperties())
        {
            BinaryExpression equalExpr;
            //other的prop属性的值
            object? otherValue = null;
            if (other != null)
            {
                otherValue = prop.GetValue(other);
            }
            Type propType = prop.PropertyType;
            //访问待比较的属性
            var leftExpr = MakeMemberAccess(
                propAccessor.Body,//要取出来Body部分，不能带参数
                prop
            );
            Expression rightExpr = Convert(Constant(otherValue), propType);
            if (propType.IsPrimitive)//基本数据类型和复杂类型比较方法不一样
            {
                equalExpr = Equal(leftExpr, rightExpr);
            }
            else
            {
                equalExpr = MakeBinary(ExpressionType.Equal,
                    leftExpr, rightExpr, false,
                    prop.PropertyType.GetMethod("op_Equality")
                );
            }
            if (conditionalExpr == null)
            {
                conditionalExpr = equalExpr;
            }
            else
            {
                conditionalExpr = AndAlso(conditionalExpr, equalExpr);
            }
        }
        if (conditionalExpr == null)
        {
            throw new ArgumentException("There should be at least one property.");
        }
        return Lambda<Func<TItem, bool>>(conditionalExpr, e1);
    }
}