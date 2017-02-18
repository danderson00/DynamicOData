using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using DynamicOData.Schema;
using System;
using System.Linq;

namespace DynamicOData
{
    public class EdmModelFactory
    {
        public static EdmModel Generate(Table table)
        {
            var model = new EdmModel();
            var entity = GenerateEntity(table);

            model.AddElement(entity);
            model.AddElement(GenerateContainer(table, entity));

            return model;
        }

        private static EdmEntityType GenerateEntity(Table table)
        {
            var entity = new EdmEntityType("", table.Name);
            table.Columns.ToList().ForEach(column => entity.AddStructuralProperty(column.Name, MapType(column.Type)));
            return entity;
        }

        private static EdmEntityContainer GenerateContainer(Table table, EdmEntityType entity)
        {
            var container = new EdmEntityContainer("", "DefaultContainer");
            container.AddEntitySet(table.Name, entity);
            return container;
        }

        private static IEdmPrimitiveTypeReference MapType(Type type)
        {
            var edmTypeName = 
                type.Name == "Byte[]" ? "Binary" : 
                //type.Name == "DateTimeOffset" ? "DateTime" : // some data sources don't support DateTimeOffset. Requires unit tests to ensure integrity
                type.Name;
            return EdmCoreModel.Instance.GetPrimitive(EdmCoreModel.Instance.GetPrimitiveTypeKind(edmTypeName), true);
        }
    }
}
