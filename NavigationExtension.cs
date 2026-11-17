using System;
using System.Collections.Generic;
using System.Text;

namespace PreciousPractice;

public static class NavigationExtensions
{
    // Shared by every page: true while a "go back" is in progress
    private static bool _isGoingBack;

    // Like PopAsync, but ignores extra taps while already going back
    public static async Task SafePopAsync(this INavigation navigation)
    {
        if (_isGoingBack)
            return;

        _isGoingBack = true;

        try
        {
            await navigation.PopAsync();
        }
        finally
        {
            _isGoingBack = false;
        }
    }
}