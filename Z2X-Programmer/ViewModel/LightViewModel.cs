﻿/*

Z2X-Programmer
Copyright (C) 2024
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
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using Z2XProgrammer.Converter;
using Z2XProgrammer.DataModel;
using Z2XProgrammer.DataStore;
using Z2XProgrammer.Helper;
using Z2XProgrammer.Messages;

namespace Z2XProgrammer.ViewModel
{
    public partial class LightViewModel : ObservableObject
    {
        #region REGION: DATASTORE & SETTINGS

        // dataStoreDataValid is TRUE if current decoder settings are available
        // (e.g. a Z2X project has been loaded or a decoder has been read out).
        [ObservableProperty]
        bool dataStoreDataValid;

        // additionalDisplayOfCVValues is true if the user-specific option xxx is activated.
        [ObservableProperty]
        bool additionalDisplayOfCVValues = int.Parse(Preferences.Default.Get(AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_KEY, AppConstants.PREFERENCES_ADDITIONALDISPLAYOFCVVALUES_VALUE)) == 1 ? true : false;

        #endregion        

        #region REGION: DECODER FEATURES
        
        // ZIMO: Light effects in CV125 and CV126 (ZIMO_LIGHT_EFFECTS_CV125X).
        [ObservableProperty]
        bool zIMO_LIGHT_EFFECTS_CV125X;

        // ZIMO: Light dimming in CV60 (ZIMO_LIGHT_DIM_CV60).
        [ObservableProperty]
        bool zIMO_LIGHT_DIM_CV60;

        // ZIMO: Time settings for light effetcs in CV190 and CV191 (ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X).
        [ObservableProperty]
        bool zIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X = false;

        #endregion

        #region REGION: PUBLIC PROPERTIES

        [ObservableProperty]
        bool anyDecoderFeatureAvailable;

        // ZIMO: Time settings for light effetcs in CV190 and CV191 (ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X).
        [ObservableProperty]
        byte zIMOLightEffectFadeInTime;
        partial void OnZIMOLightEffectFadeInTimeChanged(byte value)
        {
            DecoderConfiguration.ZIMO.MSMNLightEffectFadeInTime = value;
            ZIMOLightEffectFadeInTimeLabelText = GetFadeInOutTimeLabelText(value);
            CV190Configuration = Subline.Create(new List<uint>{190});
        }

        [ObservableProperty]
        string cV190Configuration = Subline.Create(new List<uint>{190});

        [ObservableProperty]
        string zIMOLightEffectFadeInTimeLabelText = string.Empty;

        [ObservableProperty]
        byte zIMOLightEffectFadeOutTime;
        partial void OnZIMOLightEffectFadeOutTimeChanged(byte value)
        {
            DecoderConfiguration.ZIMO.MSMNLightEffectFadeOutTime = value;
            ZIMOLightEffectFadeOutTimeLabelText = GetFadeInOutTimeLabelText(value);
            CV191Configuration = Subline.Create(new List<uint>{191});
        }

        [ObservableProperty]
        string cV191Configuration = Subline.Create(new List<uint>{191});

        [ObservableProperty]
        string zIMOLightEffectFadeOutTimeLabelText = string.Empty;


        //  ZIMO: Light effects in CV125 and CV126 (ZIMO_LIGHT_EFFECTS_CV125X)
        [ObservableProperty]
        internal ObservableCollection<string> availableZIMOLightEffects = new ObservableCollection<string>();

        [ObservableProperty]
        internal ObservableCollection<string> availableZIMOLightEffectsDirections = new ObservableCollection<string>();
        
        // ZIMO: Light effects in CV125 for function output F0 (front headlight)
        [ObservableProperty]
        internal string selectedZIMOLightEffect0v = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutput0v);
        partial void OnSelectedZIMOLightEffect0vChanged(string value)
        {       
            DecoderConfiguration.ZIMO.LightEffectOutput0v = ZIMOEnumConverter.GetLightEffectType(value);
            CV125Configuration = Subline.Create(new List<uint> { 125 });
        }
        [ObservableProperty]
        internal string selectedZIMOLightEffectDirection0v = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutput0v);
        partial void OnSelectedZIMOLightEffectDirection0vChanged(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutput0v = ZIMOEnumConverter.GetLightEffectDirectionType(value);
            CV125Configuration = Subline.Create(new List<uint> { 125 });
        }

        [ObservableProperty]
        string cV125Configuration = Subline.Create(new List<uint>{125});

        // ZIMO: Light effects in CV126 for rear headlight (default F0 reverse)
        [ObservableProperty]
        internal string selectedZIMOLightEffect0r = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutput0r);
        partial void OnSelectedZIMOLightEffect0rChanged(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutput0r = ZIMOEnumConverter.GetLightEffectType(value);
            CV126Configuration = Subline.Create(new List<uint> { 126 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirection0r = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutput0r);
        partial void OnSelectedZIMOLightEffectDirection0rChanged(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutput0r = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV126Configuration = Subline.Create(new List<uint> { 126 });
        }

        [ObservableProperty]
        string cV126Configuration = Subline.Create(new List<uint>{126});

        // ZIMO: Light effects in CV127 for FA1
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA1 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA1);
        partial void OnSelectedZIMOLightEffectFA1Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA1 = ZIMOEnumConverter.GetLightEffectType(value);
            CV127Configuration = Subline.Create(new List<uint> { 127 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA1 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA1);
        partial void OnSelectedZIMOLightEffectDirectionFA1Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA1  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV127Configuration = Subline.Create(new List<uint> { 127 });
        }

        [ObservableProperty]
        string cV127Configuration = Subline.Create(new List<uint>{127});

        // ZIMO: Light effects in CV128 for FA2
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA2 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA2);
        partial void OnSelectedZIMOLightEffectFA2Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA2 = ZIMOEnumConverter.GetLightEffectType(value);
            CV128Configuration = Subline.Create(new List<uint> { 128 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA2 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA2);
        partial void OnSelectedZIMOLightEffectDirectionFA2Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA2  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV128Configuration = Subline.Create(new List<uint> { 128 });
        }

        [ObservableProperty]
        string cV128Configuration = Subline.Create(new List<uint>{128});

        // ZIMO: Light effects in CV129 for FA3
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA3 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA3);
        partial void OnSelectedZIMOLightEffectFA3Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA3 = ZIMOEnumConverter.GetLightEffectType(value);
            CV129Configuration = Subline.Create(new List<uint> { 129 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA3 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA3);
        partial void OnSelectedZIMOLightEffectDirectionFA3Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA3  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV129Configuration = Subline.Create(new List<uint> { 129 });
        }

        [ObservableProperty]
        string cV129Configuration = Subline.Create(new List<uint>{129});

        // ZIMO: Light effects in CV130 for FA4
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA4 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA4);
        partial void OnSelectedZIMOLightEffectFA4Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA4 = ZIMOEnumConverter.GetLightEffectType(value);
            CV130Configuration = Subline.Create(new List<uint> { 130 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA4 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA4);
        partial void OnSelectedZIMOLightEffectDirectionFA4Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA4  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV130Configuration = Subline.Create(new List<uint> { 130 });
        }

        [ObservableProperty]
        string cV130Configuration = Subline.Create(new List<uint>{130});

        // ZIMO: Light effects in CV131 for FA5
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA5 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA5);
        partial void OnSelectedZIMOLightEffectFA5Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA5 = ZIMOEnumConverter.GetLightEffectType(value);
            CV131Configuration = Subline.Create(new List<uint> { 131 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA5 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA5);
        partial void OnSelectedZIMOLightEffectDirectionFA5Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA5  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV131Configuration = Subline.Create(new List<uint> { 131 });
        }

        [ObservableProperty]
        string cV131Configuration = Subline.Create(new List<uint>{131});

        // ZIMO: Light effects in CV132 for FA6
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA6 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA6);
        partial void OnSelectedZIMOLightEffectFA6Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA6 = ZIMOEnumConverter.GetLightEffectType(value);
            CV132Configuration = Subline.Create(new List<uint> { 132 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA6 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA6);
        partial void OnSelectedZIMOLightEffectDirectionFA6Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA6  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV132Configuration = Subline.Create(new List<uint> { 132 });
        }

        [ObservableProperty]
        string cV132Configuration = Subline.Create(new List<uint>{132});

        // ZIMO: Light effects in CV159 for FA7
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA7 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA7);
        partial void OnSelectedZIMOLightEffectFA7Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA7 = ZIMOEnumConverter.GetLightEffectType(value);
            CV159Configuration = Subline.Create(new List<uint> { 159 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA7 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA7);
        partial void OnSelectedZIMOLightEffectDirectionFA7Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA7  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV159Configuration = Subline.Create(new List<uint> { 159 });
        }

        [ObservableProperty]
        string cV159Configuration = Subline.Create(new List<uint>{159});

        // ZIMO: Light effects in CV160 for FA8
        [ObservableProperty]
        internal string selectedZIMOLightEffectFA8 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA8);
        partial void OnSelectedZIMOLightEffectFA8Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectOutputFA8 = ZIMOEnumConverter.GetLightEffectType(value);
            CV160Configuration = Subline.Create(new List<uint> { 160 });
        }

        [ObservableProperty]
        internal string selectedZIMOLightEffectDirectionFA8 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA8);
        partial void OnSelectedZIMOLightEffectDirectionFA8Changed(string value)
        {
            DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA8  = ZIMOEnumConverter.GetLightEffectDirectionType(value);  
            CV160Configuration = Subline.Create(new List<uint> { 160 });
        }

        [ObservableProperty]
        string cV160Configuration = Subline.Create(new List<uint>{160});






        // ZIMO: Light dimming in CV60 (ZIMO_LIGHT_DIM_CV60).
        [ObservableProperty]
        bool dimmingEnabled;
        partial void OnDimmingEnabledChanged(bool value)
        {
            //  We check if the user would like to use the dimming function.
            if (value == true)
            {
                // The user would like to use the dimming function. We check if we already have a valid value for the brightness,
                // if not we set it to 170. According to the ZIMO manual, the default value 170 =  2/3 of full voltage.
                if (DecoderConfiguration.ZIMO.DimmingFunctionOutputMasterValue == 0) Brightness = 170;
            }
            else
            {
                // The user would like to disable the dimming function. We set the brightness to 0.
                Brightness = 0;
            }
            CV60Configuration = Subline.Create(new List<uint>{60});
        }

        [ObservableProperty]
        string cV60Configuration = Subline.Create(new List<uint>{60});

        [ObservableProperty]
        bool dimmingOutput0frontEnabled;
        partial void OnDimmingOutput0frontEnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 0, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }
        [ObservableProperty]
        string cV114Configuration = Subline.Create(new List<uint>{114});


        [ObservableProperty]
        bool dimmingOutput0rearEnabled;
        partial void OnDimmingOutput0rearEnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 1, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }

        [ObservableProperty]
        bool dimmingOutput1Enabled;
        partial void OnDimmingOutput1EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 2, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }

        [ObservableProperty]
        bool dimmingOutput2Enabled;
        partial void OnDimmingOutput2EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 3, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }

        [ObservableProperty]
        bool dimmingOutput3Enabled;
        partial void OnDimmingOutput3EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 4, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }

        [ObservableProperty]
        bool dimmingOutput4Enabled;
        partial void OnDimmingOutput4EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 5, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }

        [ObservableProperty]
        bool dimmingOutput5Enabled;
        partial void OnDimmingOutput5EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 6, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }

        [ObservableProperty]
        bool dimmingOutput6Enabled;
        partial void OnDimmingOutput6EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 7, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled = temp;
            CV114Configuration = Subline.Create(new List<uint>{114});
        }

        [ObservableProperty]
        bool dimmingOutput7Enabled;
        partial void OnDimmingOutput7EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 0, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled = temp;
            CV152Configuration = Subline.Create(new List<uint>{152});
        }
        [ObservableProperty]
        string cV152Configuration = Subline.Create(new List<uint>{152});

        [ObservableProperty]
        bool dimmingOutput8Enabled;
        partial void OnDimmingOutput8EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 1, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled = temp;
            CV152Configuration = Subline.Create(new List<uint>{152});
        }

        [ObservableProperty]
        bool dimmingOutput9Enabled;
        partial void OnDimmingOutput9EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 2, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled = temp;
            CV152Configuration = Subline.Create(new List<uint>{152});
        }

        [ObservableProperty]
        bool dimmingOutput10Enabled;
        partial void OnDimmingOutput10EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 3, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled = temp;
            CV152Configuration = Subline.Create(new List<uint>{152});
        }

        [ObservableProperty]
        bool dimmingOutput11Enabled;
        partial void OnDimmingOutput11EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 4, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled = temp;
            CV152Configuration = Subline.Create(new List<uint>{152});
        }

        [ObservableProperty]
        bool dimmingOutput12Enabled;
        partial void OnDimmingOutput12EnabledChanged(bool value)
        {
            byte temp = DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled;
            temp = Bit.Set(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 5, !value);
            DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled = temp;
            CV152Configuration = Subline.Create(new List<uint>{152});
        }


        [ObservableProperty]
        byte brightness;
        partial void OnBrightnessChanged(byte value)
        {
            DecoderConfiguration.ZIMO.DimmingFunctionOutputMasterValue = value;
            BrightnessLabelText = GetBrightnessLabelText();
            CV60Configuration = Subline.Create(new List<uint> { 60 });
        }

        [ObservableProperty]
        string brightnessLabelText = "";

        #endregion

        #region REGION: CONSTRUCTOR
        public LightViewModel()
        {
            OnGetDecoderConfiguration();
            OnGetDataFromDecoderSpecification();

            WeakReferenceMessenger.Default.Register<DecoderConfigurationUpdateMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDecoderConfiguration();
                });
            });

            WeakReferenceMessenger.Default.Register<DecoderSpecificationUpdatedMessage>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    OnGetDataFromDecoderSpecification();
                });
            });

        }
        #endregion

        #region REGION: PRIVATE FUNCTIONS

        /// <summary>
        /// Converts the current brightness value of CV60 to a label text (value + percentage).
        /// </summary>
        /// <returns></returns>
        private string GetBrightnessLabelText()
        {
        
            float percentage = ((float)100 / (float)255) * (float)Brightness;
            return Brightness.ToString() + " (" + string.Format("{0:N0}", percentage) + " %)";
        }

        /// <summary>
        /// Converts the given light effect fade in/out time to a label text.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetFadeInOutTimeLabelText(int value)
        {
            try
            {
                if ((value >= 0) && (value <= 100))
                {
                    return ((float)(value) / 100).ToString() + " s";
                }
                else if ((value >= 101) && (value <= 200))
                {
                    return ((float)(value - 100)).ToString() + " s";
                }
                else if ((value >= 201) && (value <= 255))
                {
                    return string.Format("{0:N0}", Mathematics.ScaleRange(value, 201, 255, 100, 320)) + " s";
                }
                else
                {
                    return "? s";
                }
            }
            catch (Exception)
            {
                return "? s";
            }

        }


        /// <summary>
        /// The OnGetDecoderConfiguration message handler is called when the DecoderConfigurationUpdateMessage message has been received.
        /// OnGetDecoderConfiguration updates the local variables with the new decoder configuration.
        /// </summary>
        private void OnGetDecoderConfiguration()
        {
            DataStoreDataValid = DecoderConfiguration.IsValid;
            DimmingEnabled = DecoderConfiguration.ZIMO.DimmingFunctionOutputMasterValue == 0 ? false : true;
            Brightness = DecoderConfiguration.ZIMO.DimmingFunctionOutputMasterValue;
            BrightnessLabelText = GetBrightnessLabelText();
            DimmingOutput0frontEnabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 0);
            DimmingOutput0rearEnabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 1);
            DimmingOutput1Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 2);
            DimmingOutput2Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 3);
            DimmingOutput3Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 4);
            DimmingOutput4Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 5);
            DimmingOutput5Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 6);
            DimmingOutput6Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA0FA06OutputsEnabled, 7);
            DimmingOutput7Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 0);
            DimmingOutput8Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 1);
            DimmingOutput9Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 2);
            DimmingOutput10Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 3);
            DimmingOutput11Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 4);
            DimmingOutput12Enabled = !Bit.IsSet(DecoderConfiguration.ZIMO.DimmingFunctionFA7FA12OutputsEnabled, 5);

            AvailableZIMOLightEffects = new ObservableCollection<String>(ZIMOEnumConverter.GetAvailableLightEffects());
            AvailableZIMOLightEffectsDirections = new ObservableCollection<String>(ZIMOEnumConverter.GetAvailableLightEffectDirections());

            SelectedZIMOLightEffect0v = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutput0v);
            SelectedZIMOLightEffect0r = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutput0r);
            SelectedZIMOLightEffectFA1 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA1);
            SelectedZIMOLightEffectFA2 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA2);
            SelectedZIMOLightEffectFA3 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA3);
            SelectedZIMOLightEffectFA4 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA4);
            SelectedZIMOLightEffectFA5 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA5);
            SelectedZIMOLightEffectFA6 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA6);
            SelectedZIMOLightEffectFA7 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA7);
            SelectedZIMOLightEffectFA8 = ZIMOEnumConverter.GetLightEffectDescription(DecoderConfiguration.ZIMO.LightEffectOutputFA8);

            SelectedZIMOLightEffectDirection0v = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutput0v);
            SelectedZIMOLightEffectDirection0r = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutput0r);
            SelectedZIMOLightEffectDirectionFA1 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA1);
            SelectedZIMOLightEffectDirectionFA2 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA2);
            SelectedZIMOLightEffectDirectionFA3 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA3);
            SelectedZIMOLightEffectDirectionFA4 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA4);
            SelectedZIMOLightEffectDirectionFA5 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA5);
            SelectedZIMOLightEffectDirectionFA6 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA6);
            SelectedZIMOLightEffectDirectionFA7 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA7);
            SelectedZIMOLightEffectDirectionFA8 = ZIMOEnumConverter.GetLightEffectDirectionDescription(DecoderConfiguration.ZIMO.LightEffectDirectionOutputFA8);


            // ZIMO: Time settings for light effetcs in CV190 and CV191 (ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X).
            ZIMOLightEffectFadeInTime = DecoderConfiguration.ZIMO.MSMNLightEffectFadeInTime;
            ZIMOLightEffectFadeInTimeLabelText = GetFadeInOutTimeLabelText(ZIMOLightEffectFadeInTime);
            ZIMOLightEffectFadeOutTime = DecoderConfiguration.ZIMO.MSMNLightEffectFadeOutTime;
            ZIMOLightEffectFadeOutTimeLabelText = GetFadeInOutTimeLabelText(ZIMOLightEffectFadeOutTime);


        }

        /// <summary>
        /// The OnGetDataFromDecoderSpecification message handler is called when the DecoderSpecificationUpdatedMessage message has been received.
        /// OnGetDataFromDecoderSpecification updates the local variables with the new decoder specification.
        /// </summary>
        private void OnGetDataFromDecoderSpecification()
        {
            ZIMO_LIGHT_DIM_CV60 = DecoderSpecification.ZIMO_LIGHT_DIM_CV60;
            ZIMO_LIGHT_EFFECTS_CV125X = DecoderSpecification.ZIMO_LIGHT_EFFECTS_CV125X;
            ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X = DecoderSpecification.ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X;


            if ((ZIMO_LIGHT_DIM_CV60 == true) || (ZIMO_LIGHT_EFFECTS_CV125X == true) || (ZIMO_MSMNBRIGHTENINGUPANDIMMINGTIMES_CV190X == true))
            {
                AnyDecoderFeatureAvailable = true;
            }
            else
            {
                AnyDecoderFeatureAvailable = false;
            }
        }

        #endregion
    }
}
