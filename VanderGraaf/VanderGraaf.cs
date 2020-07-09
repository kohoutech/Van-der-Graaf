﻿/* ----------------------------------------------------------------------------
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

using VDG;

namespace VanderGraaf
{
    class VanderGraaf
    {
        public String templatename;
        public String contentname;
        public String outputname;

        public VanderGraaf(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("syntax: vandergraaf <template file> <content file> <output file>");
                System.Environment.Exit(1);
            }

            templatename = args[0];
            contentname = args[1];
            outputname = args[2];
        }

        public void generate()
        {
            Vander vander = new Vander();

            vander.setTemplateFile(templatename);
            vander.setContentFile(contentname);
            vander.setOutputFile(outputname);

            vander.generate();
        }

        static void Main(string[] args)
        {
            VanderGraaf vdg = new VanderGraaf(args);
            vdg.generate();
        }
    }
}

//Console.WriteLine("there's no sun in the shadow of the Wizard");