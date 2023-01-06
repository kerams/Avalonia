using System;
using Avalonia.Media;
using Avalonia.Native.Interop;
using Avalonia.Platform;

namespace Avalonia.Native;

internal class NativePlatformSettings : DefaultPlatformSettings
{
    private readonly IAvnPlatformSettings _platformSettings;
    private PlatformColorValues _lastColorValues;

    public NativePlatformSettings(IAvnPlatformSettings platformSettings)
    {
        _platformSettings = platformSettings;
        platformSettings.RegisterColorsChange(new ColorsChangeCallback(this));
    }

    public override PlatformColorValues GetColorValues()
    {
        var theme = (PlatformThemeVariant)_platformSettings.PlatformTheme;
        var color = _platformSettings.AccentColor;

        if (color > 0)
        {
            _lastColorValues = new PlatformColorValues(theme, Color.FromUInt32(color));
        }
        else
        {
            _lastColorValues = new PlatformColorValues(theme);
        }

        return _lastColorValues;
    }

    public void OnColorValuesChanged()
    {
        var oldColorValues = _lastColorValues;
        var colorValues = GetColorValues();

        if (oldColorValues != colorValues)
        {
            OnColorValuesChanged(colorValues);
        }
    }

    private class ColorsChangeCallback : NativeCallbackBase, IAvnActionCallback
    {
        private readonly NativePlatformSettings _settings;

        public ColorsChangeCallback(NativePlatformSettings settings)
        {
            _settings = settings;
        }
        
        public void Run()
        {
            _settings.OnColorValuesChanged();
        }
    }
}
