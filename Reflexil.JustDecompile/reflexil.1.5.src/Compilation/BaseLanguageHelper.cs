/* Reflexil Copyright (c) 2007-2012 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

#region " Imports "
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;
#endregion

namespace Reflexil.Compilation
{
    /// <summary>
    /// Base class for code generation
    /// </summary>
    internal abstract class BaseLanguageHelper : IReflectionVisitor, ILanguageHelper
    {

        #region " Constants "
        protected const string BASIC_SEPARATOR = ", ";
        protected const string GENERIC_TYPE_TAG = "`";
        protected const string LEFT_PARENTHESIS = "(";
        protected const string RIGHT_PARENTHESIS = ")";
        protected const string LEFT_BRACE = "{";
        protected const string RIGHT_BRACE = "}";
        protected const string LEFT_CHEVRON = "<";
        protected const string RIGHT_CHEVRON = ">";
        protected const string LEFT_BRACKET = "[";
        protected const string RIGHT_BRACKET = "]";
        protected const string REFERENCE_TYPE_TAG = "&";
        protected const string NAMESPACE_SEPARATOR = ".";
        protected const string SPACE = " ";
        protected const string QUOTE = "\"";
        protected string[] DEFAULT_NAMESPACES = { "System", "System.Collections.Generic", "System.Text" };
        #endregion

        #region " Fields "
        private StringBuilder m_identedbuilder = new StringBuilder();
        private int m_identlevel = 0;
        protected Dictionary<string, string> m_aliases = new Dictionary<string, string>();
        protected bool m_fullnamespaces = true;
        #endregion

        #region " Properties "
        protected int IdentLevel
        {
            get
            {
                return m_identlevel;
            }
            set
            {
                m_identlevel = value;
            }
        }
        #endregion

        #region " Methods "

        #region " Abstract "
        /// <summary>
        /// Write a method signature to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        protected abstract void WriteMethodSignature(MethodDefinition mdef);

        /// <summary>
        /// Write a method body to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        protected abstract void WriteMethodBody(MethodDefinition mdef);

        /// <summary>
        /// Write a type signature to the text buffer
        /// </summary>
        /// <param name="tdef">Type definition</param>
        protected abstract void WriteTypeSignature(TypeDefinition tdef);

        /// <summary>
        /// Write a field to the text buffer
        /// </summary>
        /// <param name="fdef">Field definition</param>
        protected abstract void WriteField(FieldDefinition fdef);

        /// <summary>
        /// Write a comment to the text buffer
        /// </summary>
        /// <param name="comment">Comment</param>
        protected abstract void WriteComment(string comment);

        /// <summary>
        /// Write fields stubs to the text buffer
        /// </summary>
        /// <param name="fields">Fields stubs</param>
        protected abstract void WriteFieldsStubs(Mono.Collections.Generic.Collection<FieldDefinition> fields);

        /// <summary>
        /// Write methods stubs to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition to exclude</param>
        /// <param name="methods">Methods definitions</param>
        protected abstract void WriteMethodsStubs(MethodDefinition mdef, Mono.Collections.Generic.Collection<MethodDefinition> methods);

        /// <summary>
        /// Write default namespaces to the text buffer
        /// </summary>
        protected abstract void WriteDefaultNamespaces();

        /// <summary>
        /// Write a method's owner type to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        protected abstract void WriteType(MethodDefinition mdef);

        /// <summary>
        /// Write referenced assemblies to the text buffer (as a comment)
        /// </summary>
        /// <param name="references">Assembly references</param>
        protected abstract void WriteReferencedAssemblies(List<AssemblyNameReference> references);
        #endregion

        #region " IReflectionVisitor "
        /// <summary>
        /// Visit a type definition
        /// </summary>
        /// <param name="type">Type definition</param>
        public abstract void VisitTypeDefinition(TypeDefinition type);

        /// <summary>
        /// Visit a field definition
        /// </summary>
        /// <param name="field">Field definition</param>
        public abstract void VisitFieldDefinition(FieldDefinition field);

        /// <summary>
        /// Visit a method definition
        /// </summary>
        /// <param name="method">Method definition</param>
        public abstract void VisitMethodDefinition(MethodDefinition method);

        /// <summary>
        /// Visit a type reference
        /// </summary>
        /// <param name="type">Type reference</param>
        public abstract void VisitTypeReference(TypeReference type);

        /// <summary>
        /// Visit a generic parameter collection
        /// </summary>
        /// <param name="genparams">Generic parameter collection</param>
        public abstract void VisitGenericParameterCollection(Mono.Collections.Generic.Collection<GenericParameter> genparams);

        /// <summary>
        /// Visit a parameter definition
        /// </summary>
        /// <param name="parameter">Parameter definition</param>
        public abstract void VisitParameterDefinition(ParameterDefinition parameter);

        /// <summary>
        /// Visit a parameter definition collection
        /// </summary>
        /// <param name="parameters"></param>
        public abstract void VisitParameterDefinitionCollection(Mono.Collections.Generic.Collection<ParameterDefinition> parameters);
        #endregion

        #region " Not Implemented "
        public virtual void VisitCustomAttributeCollection(Mono.Collections.Generic.Collection<CustomAttribute> customAttrs) { }
        public virtual void VisitSecurityDeclaration(SecurityDeclaration secDecl) { }
        public virtual void VisitCustomAttribute(CustomAttribute customAttr) { }
        public virtual void VisitSecurityDeclarationCollection(Mono.Collections.Generic.Collection<SecurityDeclaration> secDecls) { }
        public virtual void VisitOverride(MethodReference ov) { }
        public virtual void VisitOverrideCollection(Mono.Collections.Generic.Collection<MethodReference> meth) { }
        public virtual void VisitGenericParameter(GenericParameter genparam) { }
        public virtual void VisitConstructorCollection(Mono.Collections.Generic.Collection<MethodDefinition> ctors) { }
        public virtual void VisitEventDefinitionCollection(Mono.Collections.Generic.Collection<EventDefinition> events) { }
        public virtual void VisitFieldDefinitionCollection(Mono.Collections.Generic.Collection<FieldDefinition> fields) { }
        public virtual void VisitMethodDefinitionCollection(Mono.Collections.Generic.Collection<MethodDefinition> methods) { }
        public virtual void VisitNestedTypeCollection(Mono.Collections.Generic.Collection<TypeDefinition> nestedTypes) { }
        public virtual void VisitPropertyDefinitionCollection(Mono.Collections.Generic.Collection<PropertyDefinition> properties) { }
        public virtual void VisitTypeDefinitionCollection(Mono.Collections.Generic.Collection<TypeDefinition> types) { }
        public virtual void VisitEventDefinition(EventDefinition evt) { }
        public virtual void VisitModuleDefinition(ModuleDefinition @module) { }
        public virtual void VisitNestedType(TypeDefinition nestedType) { }
        public virtual void VisitPropertyDefinition(PropertyDefinition @property) { }
        public virtual void VisitConstructor(MethodDefinition ctor) { }
        public virtual void TerminateModuleDefinition(ModuleDefinition @module) { }
        public virtual void VisitExternType(TypeReference externType) { }
        public virtual void VisitExternTypeCollection(Mono.Collections.Generic.Collection<TypeReference> externs) { }
        public virtual void VisitInterface(TypeReference interf) { }
        public virtual void VisitInterfaceCollection(Mono.Collections.Generic.Collection<TypeReference> interfaces) { }
        public virtual void VisitMemberReference(MemberReference member) { }
        public virtual void VisitMemberReferenceCollection(Mono.Collections.Generic.Collection<MemberReference> members) { }
        public virtual void VisitMarshalSpec(MarshalInfo MarshalInfo) { }
        public virtual void VisitTypeReferenceCollection(Mono.Collections.Generic.Collection<TypeReference> refs) { }
        public virtual void VisitPInvokeInfo(PInvokeInfo pinvk) { }
        #endregion

        #region " Text generation "
        /// <summary>
        /// Change ident level and apply modifications to the text buffer
        /// </summary>
        /// <param name="newidentlevel">Ident level</param>
        protected void ReIdent(int newidentlevel)
        {
            UnIdent();
            IdentLevel = newidentlevel;
            Ident();
        }

        /// <summary>
        /// Pre-ident the text buffer
        /// </summary>
        protected void Ident()
        {
            for (int i = 0; i < IdentLevel; i++)
            {
                m_identedbuilder.Append("\t");
            }
        }

        /// <summary>
        /// Unident the pre-idented text buffer
        /// </summary>
        protected void UnIdent()
        {
            for (int i = 0; i < IdentLevel; i++)
            {
                if ((m_identedbuilder.Length > 0) && (m_identedbuilder[m_identedbuilder.Length - 1] == '\t'))
                {
                    m_identedbuilder.Remove(m_identedbuilder.Length - 1, 1);
                }
            }
        }

        /// <summary>
        /// Replace text in the text buffer
        /// </summary>
        /// <param name="oldvalue">string to replace</param>
        /// <param name="newvalue">replacement string</param>
        protected void Replace(string oldvalue, string newvalue)
        {
            m_identedbuilder.Replace(oldvalue, newvalue);
        }

        /// <summary>
        /// Write a new line to the text buffer
        /// </summary>
        protected void WriteLine()
        {
            m_identedbuilder.AppendLine();
            Ident();
        }

        /// <summary>
        /// Write a string and a new line to the text buffer
        /// </summary>
        /// <param name="str">the string to write</param>
        protected void WriteLine(string str)
        {
            Write(str);
            WriteLine();
        }

        /// <summary>
        /// Write a string to the text buffer
        /// </summary>
        /// <param name="str">the string to write</param>
        protected void Write(string str)
        {
            m_identedbuilder.Append(str);
        }

        /// <summary>
        /// Write an item to the text buffer (space surrounding)
        /// </summary>
        /// <param name="item">the item to write</param>
        /// <param name="mode">space surrounding mode</param>
        protected void Write(System.Enum item, ESpaceSurrounder mode)
        {
            m_identedbuilder.Append(Surround(item, mode));
        }

        /// <summary>
        /// Write an enum
        /// </summary>
        /// <param name="item">Item to write</param>
        protected void Write(System.Enum item)
        {
            Write(item.ToString());
        }

        /// <summary>
        /// Write an enum
        /// </summary>
        /// <param name="item">Item to write</param>
        protected void WriteLine(System.Enum item)
        {
            WriteLine(item.ToString());
        }
     
        /// <summary>
        /// Reset the text buffer 
        /// </summary>
        protected void Reset()
        {
            m_identlevel = 0;
            m_identedbuilder.Length = 0;
        }

        /// <summary>
        /// Get the text buffer
        /// </summary>
        /// <returns></returns>
        protected string GetResult()
        {
            return m_identedbuilder.ToString();
        }
        #endregion

        #region " ILanguageHelper "
        /// <summary>
        /// Generate method signature 
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetMethodSignature(MethodDefinition mdef)
        {
            Reset();
            WriteMethodSignature(mdef);
            return GetResult();
        }

        /// <summary>
        /// Generate method
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetMethod(MethodDefinition mdef)
        {
            Reset();
            WriteMethodSignature(mdef);
            WriteMethodBody(mdef);
            return GetResult();
        }

        /// <summary>
        /// Generate field
        /// </summary>
        /// <param name="fdef">Field definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetField(FieldDefinition fdef)
        {
            Reset();
            WriteField(fdef);
            return GetResult();
        }

        /// <summary>
        /// Generate type signature
        /// </summary>
        /// <param name="tdef">Type definition</param>
        /// <returns>generated source code</returns>
        public virtual string GetTypeSignature(TypeDefinition tdef)
        {
            Reset();
            WriteTypeSignature(tdef);
            return GetResult();
        }

        /// <summary>
        /// Generate source code from method declaring type. All others
        /// methods are generated as stubs.
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <param name="references">Assembly references</param>
        /// <returns>generated source code</returns>
        public abstract string GenerateSourceCode(MethodDefinition mdef, List<AssemblyNameReference> references);
        #endregion

        #region " Writers "
        /// <summary>
        /// Write methods stubs to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition to exclude</param>
        /// <param name="methods">Methods definitions</param>
        /// <param name="rskw">Region start keyword</param>
        /// <param name="rekw">Region end keyword</param>
        protected void WriteMethodsStubs(MethodDefinition mdef, Mono.Collections.Generic.Collection<MethodDefinition> methods, string rskw, string rekw)
        {
            Write(rskw);
            WriteLine("\" Methods stubs \"");
            WriteComment("Do not add or update any method. If compilation fails because of a method declaration, comment it");
            foreach (MethodDefinition smdef in methods)
            {
                if (mdef != smdef)
                {
                    WriteMethodSignature(smdef);
                    WriteMethodBody(smdef);
                    WriteLine();
                }
            }
            WriteLine(rekw);
        }

        /// <summary>
        /// Write fields stubs to the text buffer
        /// </summary>
        /// <param name="fields">Fields stubs</param>
        /// <param name="rskw">Region start keyword</param>
        /// <param name="rekw">Region end keyword</param>
        protected void WriteFieldsStubs(Mono.Collections.Generic.Collection<FieldDefinition> fields, string rskw, string rekw)
        {
            Write(rskw);
            WriteLine("\" Fields stubs \"");
            WriteComment("Do not add or update any field. If compilation fails because of a field declaration, comment it");
            foreach (FieldDefinition fdef in fields)
            {
                WriteField(fdef);
                WriteLine();
            }
            WriteLine(rekw);
        }

        /// <summary>
        /// Write referenced assemblies to the text buffer (as a comment)
        /// </summary>
        /// <param name="references">Assembly references</param>
        /// <param name="rskw">Region start keyword</param>
        /// <param name="rekw">Region end keyword</param>
        protected void WriteReferencedAssemblies(List<AssemblyNameReference> references, string rskw, string rekw)
        {
            Write(rskw);
            WriteLine("\" Referenced assemblies \"");
            foreach (AssemblyNameReference asmref in references)
            {
                WriteComment(String.Format("- {0} v{1}", asmref.Name, asmref.Version));
            }
            WriteLine(rekw);
        }
        
        /// <summary>
        /// Write a method's owner type to the text buffer
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <param name="tkw">Type keyword</param>
        /// <param name="tskw">Type start keyword</param>
        /// <param name="tekw">Type end keywork</param>
        protected void WriteType(MethodDefinition mdef, string tkw, string tskw, string tekw)
        {
            Write(tkw);
            WriteTypeSignature(mdef.DeclaringType as TypeDefinition);
            WriteLine();
            IdentLevel++;
            WriteLine(tskw);

            WriteComment("Limited support!");
            WriteComment("You can only reference methods or fields defined in the class (not in ancestors classes)");
            WriteComment("Fields and methods stubs are needed for compilation purposes only.");
            WriteComment("Reflexil will automaticaly map current type, fields or methods to original references.");
            WriteMethodSignature(mdef);
            WriteMethodBody(mdef);

            WriteLine();
            WriteMethodsStubs(mdef, (mdef.DeclaringType as TypeDefinition).Methods);

            WriteLine();
            WriteFieldsStubs((mdef.DeclaringType as TypeDefinition).Fields);

            ReIdent(IdentLevel - 1);
            WriteLine();
            WriteLine(tekw);
        }
        #endregion

        #region " Helpers "
        /// <summary>
        /// Replace all aliases in a string
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result string</returns>
        protected string HandleAliases(string str)
        {
            foreach (string alias in m_aliases.Keys)
            {
                str = str.Replace(alias, m_aliases[alias]);
            }
            return str;
        }

        /// <summary>
        /// Replace all keyword in a string
        /// </summary>
        /// <param name="str">Input string</param>
        /// <returns>Result string</returns>
        protected abstract string HandleKeywords(string str);

        /// <summary>
        /// Write a name to the text buffer. Handles aliases and namespaces.
        /// </summary>
        /// <param name="type">Type reference for namespace</param>
        /// <param name="name">The name to write</param>
        protected void HandleTypeName(TypeReference type, string name)
        {
            name = HandleAliases(name);

            if (!m_fullnamespaces)
            {
                name = name.Replace(type.Namespace + NAMESPACE_SEPARATOR, String.Empty);
            }

            Write(name);
        }

        /// <summary>
        /// Visit a collection which contains IReflectionVisitable items, and write it to the text buffer.
        /// </summary>
        /// <param name="start">Collection start keyword</param>
        /// <param name="end">Collection end keyword</param>
        /// <param name="separator">Collection separator</param>
        /// <param name="always">If true write 'collection end keyword' even if collection is empty</param>
        /// <param name="collection">Collection to visit</param>
        protected virtual void VisitVisitableCollection(string start, string end, string separator, bool always, ICollection collection)
        {
            if (always | collection.Count > 0)
            {
                Write(start);
            }

            bool firstloop = true;
            foreach (IReflectionVisitable item in collection)
            {
                if (!firstloop)
                {
                    Write(separator);
                }
                else
                {
                    firstloop = false;
                }
                if (item is TypeDefinition)
                {
                    VisitTypeReference(item as TypeDefinition);
                }
                else
                {
                    item.Accept(this);
                }
            }

            if (always | collection.Count > 0)
            {
                Write(end);
            }
        }

        /// <summary>
        /// Generate source code from method declaring type. All others
        /// methods are generated as stubs.
        /// </summary>
        /// <param name="mdef">Method definition</param>
        /// <param name="references">Assembly references</param>
        /// <param name="nkw">Namespace keyword</param>
        /// <param name="nskw">Namespace start keyword</param>
        /// <param name="nekw">Namespace end keyword</param>
        /// <returns>generated source code</returns>
        protected string GenerateSourceCode(MethodDefinition mdef, List<AssemblyNameReference> references, string nkw, string nskw, string nekw)
        {
            Reset();

            WriteDefaultNamespaces(); WriteLine();
            WriteReferencedAssemblies(references); WriteLine();

            if (mdef.DeclaringType.Namespace != string.Empty)
            {
                Write(nkw);
                WriteLine(mdef.DeclaringType.Namespace);
                IdentLevel++;
                WriteLine(nskw);
            }

            WriteType(mdef);

            if (mdef.DeclaringType.Namespace != string.Empty)
            {
                ReIdent(IdentLevel - 1);
                WriteLine(nekw);
            }

            return GetResult();
        }

        /// <summary>
        /// Surround an item with left or/and right space(s)
        /// </summary>
        /// <param name="item">item to surround</param>
        /// <param name="mode">left, right or both</param>
        /// <returns>surrounded item</returns>
        protected string Surround(System.Enum item, ESpaceSurrounder mode)
        {
            string result = item.ToString();
            if (mode != ESpaceSurrounder.After)
            {
                result = SPACE + result;
            }
            if (mode != ESpaceSurrounder.Before)
            {
                result = result + SPACE;
            }
            return result;
        }

        /// <summary>
        /// Do we need unsafe support ?
        /// </summary>
        /// <param name="source">type to test</param>
        /// <returns>true if unsafe</returns>
        protected bool IsUnsafe(TypeReference source)
        {
            if (source is PointerType)
            {
                return true;
            }
            if (source is TypeSpecification)
            {
                return IsUnsafe((source as TypeSpecification).ElementType);
            }
            return false;
        }

        /// <summary>
        /// Do we need unsafe support ?
        /// </summary>
        /// <param name="source">method to test</param>
        /// <returns>true if unsafe</returns>
        protected bool IsUnsafe(MethodDefinition source)
        {
            if (IsUnsafe(source.ReturnType))
            {
                return true;
            }
            foreach (ParameterDefinition param in source.Parameters)
            {
                if (IsUnsafe(param.ParameterType))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #endregion

    }
}
