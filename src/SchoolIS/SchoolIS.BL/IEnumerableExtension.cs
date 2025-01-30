using System.Collections;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace SchoolIS.BL;

public static class EnumerableExtension {
  public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values) =>
    new(values);
}

// https://stackoverflow.com/a/34908081
public static class QueryHelper {
  private static readonly MethodInfo OrderByMethod =
    typeof(Enumerable).GetMethods().Single(method =>
      method.Name == "OrderBy" && method.GetParameters().Length == 2);

  private static readonly MethodInfo OrderByDescendingMethod =
    typeof(Enumerable).GetMethods().Single(method =>
      method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

  public static bool PropertyExists<T>(this IEnumerable<T> source, string propertyName) {
    return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                                               BindingFlags.Public | BindingFlags.Instance) != null;
  }

  public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> source, string propertyName) {
    ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
    Expression orderByProperty = Expression.Property(parameterExpression, propertyName);
    LambdaExpression lambda = Expression.Lambda(orderByProperty, parameterExpression);
    var compiledLambda = lambda.Compile();

    MethodInfo genericMethod = OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
    object ret = genericMethod.Invoke(null, [source, compiledLambda])!;
    return (IEnumerable<T>)ret;
  }

  public static IEnumerable<T> OrderByPropertyDescending<T>(this IEnumerable<T> source,
    string propertyName) {
    ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
    Expression orderByProperty = Expression.Property(parameterExpression, propertyName);
    LambdaExpression lambda = Expression.Lambda(orderByProperty, parameterExpression);
    var compiledLambda = lambda.Compile();
    MethodInfo genericMethod =
      OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);

    object ret = genericMethod.Invoke(null, [source, compiledLambda])!;
    return (IEnumerable<T>)ret;
  }

  public static void GetEnumerableDifference<T>(this IEnumerable<T> enumerable1, IEnumerable<T> enumerable2, out IEnumerable<T> added, out IEnumerable<T> removed) {
    HashSet<T> set1 = [..enumerable1];
    HashSet<T> set2 = [..enumerable2];

    added = set2.Except(set1);
    removed = set1.Except(set2);
  }
}