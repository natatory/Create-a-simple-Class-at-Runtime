using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Create_a_simple_Class_at_Runtime
{
    public static class Kata
    {
        static AssemblyBuilder assemBuilder =
                 AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("RuntimeAssembly"),
                 AssemblyBuilderAccess.Run);
        static ModuleBuilder modBuilder = assemBuilder.DefineDynamicModule("DynamicModule");
        static TypeBuilder typeBuilder;
        public static bool DefineClass(string className, Dictionary<string, Type> properties,
            ref Type actualType)
        {
            if (assemBuilder.DefinedTypes.Any(x => x.Name == className))
            {
                string fullName = assemBuilder.DefinedTypes.First(x => x.Name == className).FullName;
                actualType = assemBuilder.GetType(fullName);
                return false;
            }
           
            actualType = MakeClass(className, properties);
            return true;
        }

        private static Type MakeClass(string className, Dictionary<string, Type> properties)
        {
            typeBuilder = modBuilder.DefineType(className, TypeAttributes.Public);
            foreach (KeyValuePair<string, Type> prop in properties)
            {
                FieldBuilder fldBldr = typeBuilder.DefineField(prop.Key,
                                                        prop.Value,
                                                        FieldAttributes.Private);

                PropertyBuilder propBldr = typeBuilder.DefineProperty(prop.Key,
                    PropertyAttributes.HasDefault,  prop.Value, null);
                MethodBuilder custNameGetPropMthdBldr = typeBuilder.DefineMethod("get_" + prop.Key,
                                       MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                       prop.Value,
                                       Type.EmptyTypes);
                ILGenerator custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();
                custNameGetIL.Emit(OpCodes.Ldarg_0);
                custNameGetIL.Emit(OpCodes.Ldfld, fldBldr);
                custNameGetIL.Emit(OpCodes.Ret);

                MethodBuilder custNameSetPropMthdBldr = typeBuilder.DefineMethod("set_" + prop.Key,
                                       MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                       null,
                                       new Type[] { prop.Value });
                ILGenerator custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();
                custNameSetIL.Emit(OpCodes.Ldarg_0);
                custNameSetIL.Emit(OpCodes.Ldarg_1);
                custNameSetIL.Emit(OpCodes.Stfld, fldBldr);
                custNameSetIL.Emit(OpCodes.Ret);

                propBldr.SetGetMethod(custNameGetPropMthdBldr);
                propBldr.SetSetMethod(custNameSetPropMthdBldr);
            }
            return typeBuilder.CreateType();
        }
    }
}
