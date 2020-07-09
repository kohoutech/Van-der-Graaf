/* ----------------------------------------------------------------------------
Van der Graaf - a static site generator
Copyright (C) 2005-2020 George E Greaney

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
----------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using VDG.Commands;

namespace VDG
{
    class Vander
    {
        public String templatename;
        public string[] templatesrc;

        public String contentname;
        public string[] contentsrc;

        public String outputname;
        public List<String> outputtxt;

        public Dictionary<String, Symbol> symbolTable;

        public Vander()
        {            
            initializeCommands();
        }

        //- initialization --------------------------------------------------------

        public void initializeCommands()
        {
            symbolTable = new Dictionary<string, Symbol>();
            symbolTable.Add("date", new DateCmd());
        }

        public void setTemplateFile(string _templatename)
        {
            templatename = _templatename;
        }

        public void setContentFile(string _contentname)
        {
            contentname = _contentname;
        }

        public void setOutputFile(string _outputname)
        {
            outputname = _outputname;
        }

        //- generation --------------------------------------------------------

        public void generate()
        {
            templatesrc = File.ReadAllLines(templatename);
            outputtxt = new List<string>();
            StringBuilder outline = new StringBuilder();

            foreach (string s in templatesrc)
            {
                outline.Clear();

                int i = 0;
                int pos = s.IndexOf("{{", i);
                while (pos != -1)
                {
                    int endpos = s.IndexOf("}}", pos + 2);
                    int len = (endpos - pos - 2);
                    String cmdstr = s.Substring(pos + 2, len);
                    
                    String resultstr = parseCommand(cmdstr);

                    outline.Append(s.Substring(i, pos - i));
                    outline.Append(resultstr);

                    i = endpos + 2;
                    pos = s.IndexOf("{{", i);
                }
                outline.Append(s.Substring(i));

                outputtxt.Add(outline.ToString());
            }

            File.WriteAllLines(outputname, outputtxt);
        }

        public String parseCommand(String s)
        {
            String result = "";

            int sep = s.IndexOf(':');
            String cmdstr = s.Substring(0, sep);
            String argstr = s.Substring(sep + 1);

            if (symbolTable.ContainsKey(cmdstr))
            {
                Symbol sym = symbolTable[cmdstr];
                if (sym is Command)
                {
                    //build arg list
                    List<string> args = new List<string>();
                    int i = 0;
                    int delim = argstr.IndexOf(',', i);
                    while (delim != -1)
                    {
                        if (argstr[delim - 1] != '\\')
                        {
                            int len = delim - i;
                            args.Add(argstr.Substring(i, len));
                            i = delim + 1;
                        }
                        delim = argstr.IndexOf(',', delim+1);
                    }
                    args.Add(argstr.Substring(i));

                    Command cmd = (Command)sym;
                    result = cmd.run(args);
                }
            }

            return result;
        }
    }
}
