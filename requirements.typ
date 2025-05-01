#let revisionInitial = "EO"

#let revision(initial) = {
  return underline[Last Revised: #datetime.today().display() By: *#initial*]
}

#set document(author: "Eliza Oselskyi", title: "Requirements")
#set page(paper: "us-letter", margin: 5em, footer: revision(revisionInitial))
#set heading(numbering: "1.")


#let titleize(title) = {
  set heading(numbering: none)
  set align(center)
  box(align(text(size: 20pt, underline(title)), horizon), fill: orange.lighten(60%), width: 1fr, height: 5em)

}

#titleize([= Requirements])

#set heading(numbering: "1.")
= Initial Questions
+ What is the goal of this application?
  - Provide a way to center the main view of a given drawing and reformat any
    detail/section views in a standardized way, whether in an open drawing or
    on a batch of drawings.

+ How will detail/section views be handled?
  - Ideally, for GA drawings, they will be aggregatted and sorted
    alphanumerically, then place them in the top right corner.

    - The constant for the bounding box they are allowed to be placed in will
      be deterimined later, depending on the sheet size.

    - If they run out of room, or a detail view is too big to fit into the
      bounding box, they will be left alone/excluded from the sort. For
      assembly drawings, there is currently no plan to include this feature.

+ How will it decide which view is the main view?
  - For *GA drawings*, it will look at the custom UDA "ViewType" and if it
    matches to a list of valid values, it is considered a main view.

  - For *Assembly drawings*, it will look if there is either only one view and
    set that as the main view, or if and only if there is only one view that is
    not a detail or section view, and sets that as the main.

    - For now, if, and only if, there is one valid view on a sheet, the view
      will be centered in the batch routine.

    - Provide extensibility to be able to set a particular view in a drawing to
      be the "main" and "secondary" views.

      - For the future main and secondary views, provide a set of formatting
        options (to be defined later), such as "side-by-side" and stacked, so
        they can be easily swapped between.

+ How will it decide which drawing is a candidate for centering?
  - For now, if and only if there is one valid view in a drawing

+ What kind of drawings should it be able to affect?
  - Fabrication and Assembly drawings.

+ What kind of views should it be able to center?
  - Anything deterimined to be a "main" view.

+ How will the interface be handled when in model mode?
  - Pop-up dialogs to prompt the user to continue/choose the kind of
    modification they want. Then, a report gets generated at the end of the
    routine.

+ How will the interface be handled when in drawing mode?
  - For now, a Windows Forms application will be spawned, allowing the user to
    center/shift the main drawing. There will also be an option to exclude the
    drawing from any future batch routines.


+ If drawings are allowed to be excluded from centering, how will the
  application know to do that? That is, where will the "Do not touch" flag be
  stored for any given drawing?
  - This will be stored in the drawing's "Title_3" property, for now.

+ When centering, where will the center of the view be placed on the sheet?
  - The main view will be placed as much as possible onto an adjusted/visual
    center, based off of the static blocks on the sheet. It is impossible to
    know the bounding boxes of certain drawing elements, thus finding a good
    average spot to place them is required for now.

+ What is the definition of a "good average spot"?
  - Taking the true center $C_t$ of the sheet $S$, subtracting out the
    titleblock $B_t$ from the sheet width $W$, giving us the adjusted width
    $W_a$, and subtracting out the template block $B_b$ from the height $H$ of
    $S$, giving us the adjusted height $H_a$. The difference between $W$ and $W_a$, $H$ and $H_a$ gives us the offsets $W_o$ and $H_o$,
    respectively. This gets us the adjusted sheet size $S_a$. The true center
    of sheet $S_a$ is our "good average spot" $C_a$.

    $ S_a = (W_a, H_a) = (W - B_t, H - B_b) $

    $ C_a = (C_w, C_h) = 1/2S_a = 1/2(W_a, H_a) $

    To adjust the origin $O$ of a view, we must calculate the orgin offsets $O_o$ between $C_a$
    and the view current center relative to the sheet $C_v$.

    $ C_v = (X_v, Y_v) $
    $ O_o = (X_1, Y_1) = (C_w - X_v, C_h - Y_v) $

    You can then apply the offset to the origin $O$:

    $ O = (X_o, Y_o) $
    $ O_a = (X_o + X_1, Y_o + Y_1 + B_b) $

    Note that we also added the template block $B_b$ to the height. This
    ensures that the view origin doesn't overplot with the template block.

+ How will it handle different sheet sizes?
 - The above solution should handle this.

+ How will it handle avoiding overplotting with other drawing objects?
 - The above solution should handle this.

+ How will it handle detail views, especially, when they are in the within the
  bounding box of the view to be centered?
  - If the views can not be placed in the upper right corner, shift the
    origins the same amount as the view it is in. If all else fails,
    completely ignore them and leave them as is.

+ Are multiple instances allowed of the application?
  - No, please either exit silently from the new spawned application if detects
    another instance or issue a pop-up saying that there is already another
    instance running.

= Big Picture Design
- Structure: Windows Forms application and Class Library
- Data: Drawing/View UDAs
- Users: One at a time on one application



//#set heading(numbering: "A.")
//= Interface

//+ hello


//= Business Logic
