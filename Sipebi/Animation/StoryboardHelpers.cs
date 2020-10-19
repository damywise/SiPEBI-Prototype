﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Sipebi {
  /// <summary>
  /// Animation helpers for <see cref="Storyboard"/>
  /// </summary>
  public static class StoryboardHelpers {
    #region Sliding To/From Right
    /// <summary>
    /// Adds a slide from right animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the right to start from</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideFromRight(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate from right
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
        To = new Thickness(0),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to right animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the right to end at</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideToRight(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate to right
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(0),
        To = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }
    #endregion

    #region Sliding To/From Left
    /// <summary>
    /// Adds a slide from left animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the left to start from</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideFromLeft(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate from left
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
        To = new Thickness(0),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to left animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the left to end at</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideToLeft(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate to left
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(0),
        To = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }
    #endregion

    #region Sliding To/From Bottom
    /// <summary>
    /// Adds a slide from bottom animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the bottom to start from</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideFromBottom(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate from bottom
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(0, keepMargin ? offset : 0, 0, -offset),
        To = new Thickness(0),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to bottom animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the bottom to end at</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideToBottom(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate to bottom
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(0),
        To = new Thickness(0, keepMargin ? offset : 0, 0, -offset),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }
    #endregion

    #region Sliding To/From Top
    /// <summary>
    /// Adds a slide from top animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the top to start from</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideFromTop(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate from top
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(0, keepMargin ? offset : 0, 0, offset),
        To = new Thickness(0),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a slide to top animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    /// <param name="offset">The distance to the top to end at</param>
    /// <param name="decelerationRatio">The rate of decelaration</param>
    /// <param name="keepMargin">Whether to keep the element at the same width during animation</param>
    public static void AddSlideToTop(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true) {
      //create the margin animate to top
      var animation = new ThicknessAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = new Thickness(0),
        To = new Thickness(0, keepMargin ? offset : 0, 0, offset),
        DecelerationRatio = decelerationRatio, //by experience, 0.9f looks pretty good most of the time
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }
    #endregion

    #region Fading
    /// <summary>
    /// Adds a fade in animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    public static void AddFadeIn(this Storyboard storyboard, float seconds) {
      //create the margin animate from right
      var animation = new DoubleAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = 0,
        To = 1,
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }

    /// <summary>
    /// Adds a fade out animation to the storyboard
    /// </summary>
    /// <param name="storyboard">The storyboard to add the animation to</param>
    /// <param name="seconds">The time the animation will take</param>
    public static void AddFadeOut(this Storyboard storyboard, float seconds) {
      //create the margin animate from right
      var animation = new DoubleAnimation {
        Duration = new Duration(TimeSpan.FromSeconds(seconds)),
        From = 1,
        To = 0,
      };

      //set the target property name
      Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

      //add this to the storyboard
      storyboard.Children.Add(animation);
    }
    #endregion
  }
}
