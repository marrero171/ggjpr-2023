/* Copyright © 2014 Apex Software. All rights reserved. */
namespace Apex.AI.Editor.UndoRedo
{
    public interface IViewBoundOperation
    {
        IView view { get; set; }
    }
}
