# Med-Tracker

Copyright (c) 2019 Allison Wong

This will be an ASP.NET Core Slack app to help track medication usage.  I take medications for migraines, but I should only be taking a certain number per week.  This app will ping me each day, collecting and storing information about medication usage in a SQLite database.   

## Current Features

Slash commands to:
- Initialize/cease reminders.  The app will then start pinging daily at a specified time
- Record medication usage
- Subscribe/unsubscribe to monthly reports 
- Update a day's record
- Retrieve the past 30 days of records

## Planned Features

Currently, the app stores daily and monthly data, and I would like for the monthly data to be accessible in a visual format and continually updated on a monthly basis.  I hope to add a website portion where the user can securely access a plot of their data as it is updated each month.  


