Slrr Lib
=======

This app is part of the suite designed for the Street Legal Racing: Redline game (https://en.wikipedia.org/wiki/Street_Legal_Racing:_Redline).
The programs and their development is discussed on the vStanced forums (http://vstanced.com/viewtopic.php?f=68&t=13249).

This is the base library for reading and writing file types used in SLRR this is based on manual reverse engineering so there are fields whose purpose is unknown and inaccuracies can be present.
For *SCX* and *RPK* files there are two flavors to be used one is intended for read-only but fast access (called *DirectAccess*) the other for arbitrary modification and construction (called *DynamicAccess*). The *.class* file decoding is quite crude and focused on the ability to read and edit *rpk-references*. The *HighLevel* namespace contains very specialized classes for well defined complex operations.

## Building

You'll need Visual Studio 2019 or higher to build Slrr Lib.

1. Clone this repository
2. Open the Visual Studio solution
3. Select either the target platform and build the solution (needed files will be copied over to the target directory).

## Contributing

Any contributions to the project are welcomed, it's recommended to use GitLab [merge requests](https://docs.gitlab.com/ee/gitlab-basics/add-merge-request.html).

## License

All source code in this repository is licensed under a [BSD 3-clause license](LICENSE.md).