using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Sipebi {
  /// <summary>
  /// Helpers to animate pages in specific ways. Can be used in any page (other than just base page)
  /// </summary>
  public static class PageAnimations {
    /// <summary>
    /// Slides a page in from the right
    /// </summary>
    /// <param name="page">The page to animate</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <returns></returns>
    public static async Task SlideAndFadeInFromRightAsync(this Page page, float seconds) {
      //create the storyboard
      var sb = new Storyboard();

      // Add slide from right animation
      sb.AddSlideFromRight(seconds, page.WindowWidth);

      // Add fade in animation
      sb.AddFadeIn(seconds);

      // Start animating
      sb.Begin(page);

      // Make page visible
      page.Visibility = Visibility.Visible;

      //Wait for it to finish
      await Task.Delay((int)(seconds * 1000));
    }

    /// <summary>
    /// Slides a page out to the left
    /// </summary>
    /// <param name="page">The page to animate</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <returns></returns>
    public static async Task SlideAndFadeOutToLeftAsync(this Page page, float seconds) {
      //create the storyboard
      var sb = new Storyboard();

      // Add slide to left animation
      sb.AddSlideToLeft(seconds, page.WindowWidth);

      // Add fade out animation
      sb.AddFadeOut(seconds);

      // Start animating
      sb.Begin(page);

      // Make page visible
      page.Visibility = Visibility.Visible;

      //Wait for it to finish
      await Task.Delay((int)(seconds * 1000));
    }

  }
}
