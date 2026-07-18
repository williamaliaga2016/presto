using Npgsql;
using System.Data.Common;
using System.Reflection;

namespace Data.Extensions.Repository
{
    public static class DataReaderExtensions
    {
        public static List<T> MapToListDomain<T>(this DbDataReader dr) where T : new()
        {
            List<T> RetVal = new List<T>();

            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();

            if (dr != null)
            {
                var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);

                while (dr.Read())
                {
                    T newObject = new T();

                    for (int Index = 0; Index < dr.FieldCount; Index++)
                    {
                        if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                        {
                            var Info = PropDict[dr.GetName(Index).ToUpper()];

                            if (Info != null && Info.CanWrite)
                            {
                                var Val = dr.GetValue(Index);
                                Info.SetValue(newObject, Val == DBNull.Value ? null : Val, null);
                            }
                        }
                    }

                    RetVal.Add(newObject);
                }
            }

            return RetVal;
        }

        public static T MapToDomain<T>(this DbDataReader dr) where T : new()
        {
            T RetVal = new T();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();
            try
            {
                if (dr != null)
                {
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    if (!dr.Read())
                        return RetVal; // Si no hay filas, salir
                    for (int Index = 0; Index < dr.FieldCount; Index++)
                    {
                        if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                        {
                            var Info = PropDict[dr.GetName(Index).ToUpper()];
                            if ((Info != null) && Info.CanWrite)
                            {
                                var Val = dr.GetValue(Index);
                                Info.SetValue(RetVal, (Val == DBNull.Value) ? null : Val, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RetVal;
        }

        public static DbParameterCollection ToArray<TEntity>(this DbParameterCollection parameterList, TEntity entity)
        where TEntity : class
        {
            Type obj = entity.GetType();
            PropertyInfo[] props = obj.GetProperties();

            foreach (var prop in props)
            {
                if (prop.GetIndexParameters().Length > 0)
                    continue;

                object? paramValue = prop.GetValue(entity, null);
                string paramName = $"@{prop.Name}";

                if (paramValue == null)
                {
                    parameterList.Add(new NpgsqlParameter(paramName, DBNull.Value));
                    continue;
                }

                if (paramValue is Guid guidValue)
                {
                    parameterList.Add(new NpgsqlParameter(paramName, guidValue));
                    continue;
                }

                switch (Type.GetTypeCode(paramValue.GetType()))
                {
                    case TypeCode.Boolean:
                        parameterList.Add(new NpgsqlParameter(paramName, (bool)paramValue));
                        break;

                    case TypeCode.DateTime:
                        parameterList.Add(new NpgsqlParameter(paramName, (DateTime)paramValue));
                        break;

                    case TypeCode.Int32:
                        parameterList.Add(new NpgsqlParameter(paramName, (int)paramValue));
                        break;

                    case TypeCode.Int64:
                        parameterList.Add(new NpgsqlParameter(paramName, (long)paramValue));
                        break;

                    case TypeCode.Double:
                        parameterList.Add(new NpgsqlParameter(paramName, (double)paramValue));
                        break;

                    case TypeCode.Decimal:
                        parameterList.Add(new NpgsqlParameter(paramName, (decimal)paramValue));
                        break;

                    default:
                        parameterList.Add(new NpgsqlParameter(paramName, paramValue.ToString()));
                        break;
                }
            }

            return parameterList;
        }
    }
}
