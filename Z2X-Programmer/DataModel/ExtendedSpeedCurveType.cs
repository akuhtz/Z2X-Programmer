﻿/*

Z2X-Programmer
Copyright (C) 2025
PeterK78

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see:

https://github.com/PeterK78/Z2X-Programmer?tab=GPL-3.0-1-ov-file.

*/

using CommunityToolkit.Mvvm.ComponentModel;
using Z2XProgrammer.Helper;

namespace Z2XProgrammer.DataModel
{
    public class ExtendedSpeedCurveType: ObservableObject
    {
        private ConfigurationVariableType[] cv;

        public ExtendedSpeedCurveType()
        {
             cv = new ConfigurationVariableType[NMRA.NumberOfFunctionKeys];
             for (int i = 0; i < NMRA.NumberOfFunctionKeys; i++)
             {
                cv[i] = new ConfigurationVariableType();
             }             

        }

        public ConfigurationVariableType[] CV { get { return cv; } }
    }
}
