//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
//
//  Author: 牛奶

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Telegram.Bot.Framework.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class CodeLanguageExtensions
    {
        private static readonly Dictionary<CodeLanguage, string> dict =
#if NET8_0_OR_GREATER
            [];
#else
            new Dictionary<CodeLanguage, string>();
#endif

        static CodeLanguageExtensions()
        {
            var type = typeof(CodeLanguage);
            // 获取枚举字段
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs.Length > 0)
                {
                    if (attrs[0] is DescriptionAttribute attr)
                    {
                        var value = (CodeLanguage)Enum.Parse(type, field.Name);
                        dict.Add(value, attr.Description);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetClass(this CodeLanguage language) => dict.TryGetValue(language, out var result) ? result : string.Empty;
    }

    /// <summary>
    /// 支持的代码语言类型
    /// </summary>
    /// <remarks>
    /// <seealso href="https://github.com/TelegramMessenger/libprisma#supported-languages">详细的语言类型</seealso>
    /// </remarks>
    public enum CodeLanguage
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("markup")]
        Markup,

        /// <summary>
        /// 
        /// </summary>
        [Description("css")]
        CSS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Clike,

        /// <summary>
        /// 
        /// </summary>
        [Description("regex")]
        Regex,

        /// <summary>
        /// 
        /// </summary>
        [Description("javascript")]
        JavaScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("abap")]
        ABAP,

        /// <summary>
        /// 
        /// </summary>
        [Description("abnf")]
        ABNF,

        /// <summary>
        /// 
        /// </summary>
        [Description("actionscript")]
        ActionScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("ada")]
        Ada,

        /// <summary>
        /// 
        /// </summary>
        [Description("agda")]
        Agda,

        /// <summary>
        /// 
        /// </summary>
        [Description("al")]
        AL,

        /// <summary>
        /// 
        /// </summary>
        [Description("antlr4")]
        ANTLR4,

        /// <summary>
        /// 
        /// </summary>
        [Description("apacheconf")]
        Apache_Configuration,

        /// <summary>
        /// 
        /// </summary>
        [Description("sql")]
        SQL,

        /// <summary>
        /// 
        /// </summary>
        [Description("apex")]
        Apex,

        /// <summary>
        /// 
        /// </summary>
        [Description("apl")]
        APL,

        /// <summary>
        /// 
        /// </summary>
        [Description("applescript")]
        AppleScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("aql")]
        AQL,

        /// <summary>
        /// 
        /// </summary>
        [Description("c")]
        C,

        /// <summary>
        /// 
        /// </summary>
        [Description("cpp")]
        CPlusPlus,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Arduino,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ARFF,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ARM_Assembly,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Bash,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        YAML,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Markdown,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Arturo,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        AsciiDoc,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        CSharp,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ASP_NET_CSharp,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Assembly_6502,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Atmel_AVR_Assembly,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        AutoHotkey,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        AutoIt,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        AviSynth,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Avro_IDL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        AWK,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        BASIC,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Batch,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        BBcode,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        BBj,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Bicep,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Birb,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Bison,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        BNF,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        BQN,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Brainfuck,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        BrightScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Bro,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        CFScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ChaiScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        CIL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Cilk_C,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Cilk_CPlusPlus,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Clojure,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        CMake,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        COBOL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        CoffeeScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Concurnas,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ContentSecurityPolicy,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Cooklang,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Ruby,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Crystal,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        CSV,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        CUE,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Cypher,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        D,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Dart,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        DataWeave,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        DAX,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Dhall,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Diff,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Markup_templating,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Django_Jinja2,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        DNS_zone_file,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Docker,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        DOT_Graphviz,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        EBNF,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        EditorConfig,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Eiffel,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        EJS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Elixir,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Elm,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Lua,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Embedded_Lua_templating,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ERB,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Erlang,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Excel_Formula,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        FSharp,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Factor,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        False,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Fift,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Firestore_security_rules,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Flow,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Fortran,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        FreeMarker_Template_Language,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        FunC,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GameMaker_Language,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GAP_CAS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Gcode,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GDScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GEDCOM,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        gettext,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Git,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GLSL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GN,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GNU_Linker_Script,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Go,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Go_module,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Gradle,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        GraphQL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Groovy,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Less,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Sass_SCSS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Textile,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Haml,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Handlebars,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Haskell,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Haxe,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        HCL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        HLSL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Hoon,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        HTTP_PublicKeyPins,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        HTTP_StrictTransportSecurity,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JSON,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        URI,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        HTTP,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        IchigoJam,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Icon,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ICU_Message_Format,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Idris,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        _ignore,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Inform_7,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Ini,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Io,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        J,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Java,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Scala,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PHP,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JavaDoclike,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JavaDoc,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Java_stack_trace,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Jolie,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JQ,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        TypeScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JSDoc,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        N4JS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JSON5,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JSONP,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        JS_stack_trace,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Julia,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Keepalived_Configure,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Keyman,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Kotlin,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Kusto,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        LaTeX,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Latte,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Scheme,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        LilyPond,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Liquid,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Lisp,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        LiveScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        LLVM_IR,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Log_file,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        LOLCODE,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Magma_CAS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Makefile,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Mata,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        MATLAB,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        MAXScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        MEL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Mermaid,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        METAFONT,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Mizar,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        MongoDB,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Monkey,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        MoonScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        N1QL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Nand_To_Tetris_HDL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Naninovel_Script,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        NASM,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        NEON,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Nevod,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        nginx,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Nim,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Nix,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        NSIS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ObjectiveC,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        OCaml,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Odin,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        OpenCL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        OpenQasm,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Oz,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PARI_GP,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Parser,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Pascal,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Pascaligo,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PATROL_Scripting_Language,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PCAxis,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PeopleCode,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Perl,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PHPDoc,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PlantUML,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PL_SQL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PowerQuery,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PowerShell,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Processing,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Prolog,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PromQL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        _properties,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Protocol_Buffers,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Stylus,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Twig,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Pug,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Puppet,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        PureBasic,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Python,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        QSharp,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Q_kdbPlus_database,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        QML,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Qore,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        R,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Racket,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Razor_CSharp,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        React_JSX,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        React_TSX,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Reason,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Rego,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Ren_py,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        ReScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        reST_reStructuredText,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Rip,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Roboconf,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Robot_Framework,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Rust,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        SAS,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Sass_Sass,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Shell_session,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Smali,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Smalltalk,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Smarty,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        SML,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Solidity_Ethereum,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Solution_file,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Soy_Closure_Template,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Splunk_SPL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        SQF_Status_Quo_Function_Arma_3,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Squirrel,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Stan,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Stata_Ado,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Structured_Text_IEC_611313,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        SuperCollider,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Swift,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Systemd_configuration_file,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Tact,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        T4_templating,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        T4_Text_Templates_CSharp,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        VB_Net,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        T4_Text_Templates_VB,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        TAP,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Tcl,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Template_Toolkit_2,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        TOML,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Tremor,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Type_Language,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Type_Language__Binary,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        TypoScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        UnrealScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        UO_Razor_Script,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        V,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Vala,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Velocity,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Verilog,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        VHDL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        vim,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Visual_Basic,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        WarpScript,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        WebAssembly,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Web_IDL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        WGSL,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Wiki_markup,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Wolfram_language,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Wren,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Xeora,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Xojo_REALbasic,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        XQuery,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        YANG,

        /// <summary>
        /// 
        /// </summary>
        [Description("clike")]
        Zig,
    }
}
