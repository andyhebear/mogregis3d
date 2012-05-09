using System;
using System.Collections.Generic;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System.Text;
using System.Reflection;

namespace MogreGis
{
    public class IronPythonScriptEngine : IScriptEngine
    {

        List<Script> scripts = new List<Script>();

        Microsoft.Scripting.Hosting.ScriptEngine engine = Python.CreateEngine();
        ScriptRuntime runtime;
        ScriptScope scope;

        // built-in scripts
        const string builtin =
@"
import Mogre

def color(a,b,c,d):
    return ""{0} {1} {2} {3}"".format(a,b,c,d)

def vec4(a,b,c,d):
    return ""{0} {1} {2} {3}"".format(a,b,c,d)

def vec3(a,b,c):
    return ""{0} {1} {2}"".format(a,b,c)

def vector3(a,b,c):
    return Mogre.Vector3(a,b,c);

def vec2(a,b):
    return ""{0} {1}"".format(a,b)

def attr_double(f,a):
    return f.getAttribute(a).asDouble()

def attr_string(f,a):
    return f.getAttribute(a).asString()

def attr_int(f,a):
    return f.getAttribute(a).asInt()

def attr_bool(f,a):
    return f.getAttribute(a).asBool()

def return_code(code):
    return code

";

        public IronPythonScriptEngine()
        {
            runtime = engine.Runtime;
            runtime.LoadAssembly(Assembly.GetAssembly(typeof(Mogre.Vector3)));
            scope = runtime.CreateScope();

            // install built-in scripts
            install(new Script(builtin));
        }



        public void install(Script script)
        {
            scripts.Add(script);
        }

        public ScriptResult run(Script script)
        {
            return run(script, null, null);
        }

        public ScriptResult run(Script script, FilterEnv env)
        {
            return run(script, null, env);
        }

        public ScriptResult run(Script script, Feature feature, FilterEnv env)
        {
            if (!string.IsNullOrWhiteSpace(script.Language) &&
                !script.Language.ToLowerInvariant().Contains("python"))
                return new ScriptResult(script.Code);

            scope.SetVariable("feature", feature);
            scope.SetVariable("env", env);

            StringBuilder code = new StringBuilder();
            foreach (Script sc in scripts)
            {
                code.Append(sc.Code);
            }
            code.Append(script.Code);

            ScriptSource source2 = engine.CreateScriptSourceFromString(code.ToString(), SourceCodeKind.AutoDetect);
            var res2 = source2.Execute(scope);

            return new ScriptResult(res2);
        }

        public string Language { get { return "Python"; } }
    }
}