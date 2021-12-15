# RL_Ecosystem

RL Ecosystem is a rough simulation of a forest ecosystem filled with woodland creatures whose objectives are to simply survive. 
The forest will be populated with rabbits and wolves, and perhaps more creatures in the future.
The creatures are Reinforcement Learning agents that attempt to find the optimal policy for survival given a randomly generated training environment. 
They are equipped with perception and memory modules that help them survive.
How to shape these modules to make the agents better survivors is one of the research questions this project hopes to answer.
The project also aims to elicit and observe emergent behaviours in these creatures that mirror the behaviours of equivalent living creatures.
Eventually, the project could implement a way for players to interact with the environment and the creatures, which would make it into a kind of simulation game.

## Environment
The environment is a 2D surface, which allows for a more efficient neural network training using simpler, abstracted perception modules as opposed to a 3D environment.
The training environment is randomly populated with objects that the creatures can interact with in some way, and other creatures that are either neutral or are predators to the creature.

## Objects
Objects include bushes, berry bushes, trees, rocks, and water (WIP).

## Agents
Each creature has a hunger status, food to eat in the environment, and optionally a predator. 
The neural networks generally receive perception, memory, status, previous actions, rewards, and direction information.

## Perception Modules
### Vision Module
### Audition Module
### Olfaction Module
### Memory Module
