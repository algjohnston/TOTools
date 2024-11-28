Sprint 4 promised tasks (with a checkmark by those that are completed):

    Add tier group drag and drop ✅ This was easy
    Clone a bracket from startgg into the UI ✅ This took a two whole days
    Drag and drop players in the brackets ❌ This ended up turning into click to change players and did not get done.
    Group events in the scheduler and add a way to change their start time ✅ This took about 3 hours
    Make schedule generation faster ✅ This was easy
    The select competitors page needs a seeding list and multiple selection ✅ This took a whole day
    Design GUI (Text fonts/colors/sizes, Backgrounds, Button colors, etc.) to have it look nicer ✅ This was easier said than done; went through a handful of redesigns
    Change splash screen and app icon ✅ This was also easy
    Add loading animation to splash screen; did not seem possible without adding its own screen (they seem static by design)
    Add to seeding list from bracket if not in seeding list ✅ Took a couple hours.
    Let user start and report match ❌ Not finished. 
    Add to the event url form, a placement number for when Bo5 Starts ✅ Ezpz
    Region Editor Popup ✅ It do exist now. I needed Alex's help though for the backend
    Make txt guide for rogers ✅ Hopefully this helps
    Add to seeding list from bracket if not in seeding list	✅ Took about half a day and was done by Alex
    Add a way to change player region tags ❌ not done
    When user stops match, update past match table ❌ not done
    Fix the scheduler crash when navigating to it twice	✅
    Add ability to remove urls from the event scheduler list ✅
    Fix crash when there an invalid input link ❌ Not done
    Make the lists look better; shrink event url; remove game from event list; make list items more separable ✅
    Make log in screen look better ✅

Sprint 3 Changes:

    Added GraphQL.Client and used it to get data from start.gg
    based on a url provided by the user! |っ^ᴥ^ꐦ|っ

    Added logic to make a schedule of matches in a way that will 
    try to prevent players from having to wait.

    Added logic to estimate match lengths.

    The match list now supports match reordering! ~(ȍ‿ȍ~)

    Had to remove StrawberryShake due to a very naive bug 
    where they assumed the id was always a string despite the 
    GraphQL spec saying it can be an int. |¬ᗒ□ᗕ|¬

Sprint 2 Changes:

    Added the map and got it to center on Madison, WI! ᕦ(ຈ◡ຈ)ᕤ
    
    Set up strawberry shake to interact with the graphql of start.gg! ⌐\[︶ヮ︶⌐]
    
    Added a loser's bracket to the double elimination page! \[ᵒಠ‸ಠᵒ]
    
    Added an event table. ‿︵‿︵‿ɳ༼ᵔᗜᵔ༽ɲ‿︵‿︵‿
    
    Added event list page. |(ˇ⚙͠ѽ⚙͠ ˇ)|
    
    Added a player table.
    
    Added players to the seeding list.
    
    Added a match table.
    
    Displayed the match data in the match scheduler.
