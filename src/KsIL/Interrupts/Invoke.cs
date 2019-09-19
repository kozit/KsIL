using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KsIL.Interrupts
{
    public class Invoke : Interrupt
    {

        enum codes
        {

            Load = 0x00,
            Load_mem = 0x01,
            Call = 0x02,
            Call_Static = 0x03,
            New = 0x04,
            Compile = 0x05


        }

        public Invoke()
        {

            Code = 4;

        }

        public override void Run(byte[] Parameters, CPU CPU)
        {

            codes code = (codes)Parameters[0];

            switch (code)
            {

                case codes.Load:
                    
                    if (Parameters.Length == 5)
                        Assembly.Load(CPU.Memory.GetData(BitConverter.ToUInt32(Parameters, 1)));
                    else
                        Assembly.LoadFile(Encoding.UTF8.GetString(Parameters, 1, Parameters.Length - 2));

                    break;

                case codes.Call:

                    object TheObject = CPU.Memory.GetObject(BitConverter.ToUInt32(Parameters, 1));

                    Type calledType = TheObject.GetType();
                    MethodInfo method = calledType.GetMethod(Encoding.UTF8.GetString(Parameters, 13, BitConverter.ToInt32(Parameters, 9)));
                    object Aresult = method.Invoke(TheObject, null);

                    if (BitConverter.ToUInt32(Parameters, 1) != 0)
                    {
                        CPU.Memory.SetObject(BitConverter.ToUInt32(Parameters, 1), Aresult);
                    }

                    break;

                case codes.New:

                    CPU.Memory.SetObject(BitConverter.ToUInt32(Parameters, 1), Activator.CreateInstance(Type.GetType(Encoding.UTF8.GetString(Parameters, 1, Parameters.Length - 6))));

                    break;

                case codes.Compile:

                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(Encoding.UTF8.GetString(CPU.Memory.GetData(BitConverter.ToUInt32(Parameters, 1))));

                    string assemblyName = Path.GetRandomFileName();
                    MetadataReference[] references = new MetadataReference[]
                    {
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
                    };

                    CSharpCompilation compilation = CSharpCompilation.Create(
                        assemblyName,
                        syntaxTrees: new[] { syntaxTree },
                        references: references,
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                    using (var ms = new MemoryStream())
                    {
                        EmitResult Mresult = compilation.Emit(ms);

                        if (!Mresult.Success)
                        {
                            IEnumerable<Diagnostic> failures = Mresult.Diagnostics.Where(diagnostic =>
                                diagnostic.IsWarningAsError ||
                                diagnostic.Severity == DiagnosticSeverity.Error);

                            foreach (Diagnostic diagnostic in failures)
                            {
                                Debugger.Log(diagnostic.Id + ": " + diagnostic.GetMessage(), "Invoke");
                            }

                            CPU.Memory.Set(Memory.Pointer.PROGRAM_RUNNING, 0x00);

                        }
                        else
                        {
                            ms.Seek(0, SeekOrigin.Begin);
                            Assembly.Load(ms.ToArray());
                        }
                    }

                    break;

            }



        }

    }
}
