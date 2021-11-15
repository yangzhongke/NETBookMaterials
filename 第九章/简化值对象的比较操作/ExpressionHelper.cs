using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;

class ExpressionHelper
{
	public static Expression<Func<TItem, bool>> MakeEqual<TItem, TProp>
(Expression<Func<TItem, TProp>> propAccessor, TProp? other)
	where TItem : class where TProp : class
	{
		var e1 = propAccessor.Parameters.Single();
		BinaryExpression? conditionalExpr = null;
		foreach (var prop in typeof(TProp).GetProperties())
		{
			BinaryExpression equalExpr;
			object? otherValue = null;
			if (other != null)
			{
				otherValue = prop.GetValue(other);
			}
			Type propType = prop.PropertyType;
			var leftExpr = MakeMemberAccess(propAccessor.Body, prop);
			Expression rightExpr = Convert(Constant(otherValue), propType);
			if (propType.IsPrimitive)
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