# Welcome to MoBi
![mobi](https://cloud.githubusercontent.com/assets/1041237/22438534/5b8d6b28-e6fa-11e6-9180-3d079eea356a.png)

MoBi® is a software tool for multiscale physiological modeling and simulation. 
Within the restrictions of ordinary differential equations, almost any kind of (biological) model 
can be imported or set up from scratch. Examples include biochemical reaction networks, 
compartmental disease progression models, or PBPK models. However, de novo development of a PBPK model, 
for example, is very cumbersome such that the preferred procedure is to import them from PK-Sim®. 
Importantly, MoBi® also allows for the combination of the described examples and thereby is a very powerful tool 
for modeling and simulation of multi-scale physiological systems covering molecular details on the one hand 
and whole-body architecture on the other hand.

De novo model establishment and simulation is supported by graphical tools and building blocks to support expert users. 
MoBi® uses building blocks that are grouped into Molecules, Reactions, Spatial Structures, Passive Transports, 
Observers, Events, Molecule Start Values, Parameter Start Values, and Observed Data. 
The different building blocks are described in detail in Part IV, “Working with MoBi®”. 
Building blocks out of the above-mentioned groups can be combined to generate models. 
The advantage of building blocks is that they can be reused. For example, a different set of starting values 
may define a new scenario, situation, or individual. Refine a Reaction(s) network and update it in all tissues 
where it should be considered.

## Code Status
[![Build status](https://ci.appveyor.com/api/projects/status/qgv5bpwys5snl7mk/branch/master?svg=true&passingText=master%20-%20passing)](https://ci.appveyor.com/project/open-systems-pharmacology-ci/mobi/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/Open-Systems-Pharmacology/MoBi/badge.svg?branch=develop)](https://coveralls.io/github/Open-Systems-Pharmacology/MoBi?branch=develop)

## Code of conduct
Everyone interacting in the Open Systems Pharmacology community (codebases, issue trackers, chat rooms, mailing lists etc...) is expected to follow the Open Systems Pharmacology [code of conduct](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/CODE_OF_CONDUCT.md).

## Contribution
We encourage contribution to the Open Systems Pharmacology community. Before getting started please read the [contribution guidelines](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/CONTRIBUTING.md). If you are contributing code, please be familiar with the [coding standard](https://github.com/Open-Systems-Pharmacology/Suite/blob/master/CODING_STANDARD.md).

## License
MoBi® is released under the [GPLv2 License](LICENSE).

All trademarks within this document belong to their legitimate owners.