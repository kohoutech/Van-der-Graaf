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

namespace VanderGraaf
{ 
    class Vander
    {
        public String sourcename;
        public string[] source;

        public String outname;
        public List<String> output;

        public Vander(String _sourcename, String _outname)
        {
            sourcename = _sourcename;
            outname = _outname;
        }

        public void generate()
        {
            source = File.ReadAllLines(sourcename);
            output = new List<string>();
            StringBuilder outline = new StringBuilder();

            foreach(string s in source)
            {
                outline.Clear();
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    outline.Append(c);
                }

                output.Add(outline.ToString());
            }

            File.WriteAllLines(outname, output);
        }
    }
}
