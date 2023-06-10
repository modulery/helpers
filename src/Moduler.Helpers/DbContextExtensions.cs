//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using System.Linq;

//namespace Moduler.Helpers
//{
//    //public class ContextExtensions
//    //{
//    //    public static string GetTableName<T>(this DbContext context) where T : class
//    //    {
//    //        ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

//    //        return objectContext.GetTableName<T>();
//    //    }

//    //    public static string GetTableName<T>(this ObjectContext context) where T : class
//    //    {
//    //        string sql = context.CreateObjectSet<T>().ToTraceString();
//    //        Regex regex = new Regex("FROM (?<table>.*) AS");
//    //        Match match = regex.Match(sql);

//    //        string table = match.Groups["table"].Value;
//    //        return table;
//    //    }
//    //}
//    public static class AttributeReader
//    {
//        //Get DB Table Name
//        public static string GetTableName<T>(DbContext context) where T : class
//        {
//            // We need dbcontext to access the models
//            var models = context.Model;

//            // Get all the entity types information
//            var entityTypes = models.GetEntityTypes();

//            // T is Name of class
//            var entityTypeOfT = entityTypes.First(t => t.ClrType == typeof(T));

//            var tableNameAnnotation = entityTypeOfT.GetAnnotation("Relational:TableName");
//            var TableName = tableNameAnnotation.Value.ToString();
//            return TableName;
//        }

//    }
//}
