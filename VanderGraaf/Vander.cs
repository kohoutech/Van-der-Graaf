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

using VanderGraaf.Commands;

namespace VanderGraaf
{
    class Vander
    {
        public String sourcename;
        public string[] source;

        public String outname;
        public List<String> output;

        public Dictionary<String, Symbol> symbolTable;

        public Vander(String _sourcename, String _outname)
        {
            sourcename = _sourcename;
            outname = _outname;
            
            initializeCommands();
        }

        public void initializeCommands()
        {
            symbolTable = new Dictionary<string, Symbol>();
            symbolTable.Add("date", new DateCmd());
        }

        public void generate()
        {
            source = File.ReadAllLines(sourcename);
            output = new List<string>();
            StringBuilder outline = new StringBuilder();

            foreach (string s in source)
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

                output.Add(outline.ToString());
            }

            File.WriteAllLines(outname, output);
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
