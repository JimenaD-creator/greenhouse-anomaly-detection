# Greenhouse Multi-Agent Anomaly Detection System

A Unity-based simulation project that models a multi-agent system (MAS) for autonomous detection and management of pests and diseases in a greenhouse environment.

## 🎯 Project Overview

This project addresses the critical agricultural issue of 20-40% global production loss caused by pests and diseases. We developed a simulated digital greenhouse environment where autonomous agents collaborate to detect infected fruits, remove them, and harvest mature crops—enabling precise, selective, and efficient crop management.

## 🤖 System Architecture

### Digital Environment
- **Platform**: Unity
- **Navigation**: Grid-based system
- **Elements**: Simulated greenhouse with healthy, mature, and infected fruits

### Autonomous Agents

| Agent | Role | Architecture Type | Key Function |
|-------|------|------------------|--------------|
| **Analyzer** | Detects infected fruits using vision | Deliberative | Captures image data, processes it, and marks coordinates of infected fruits |
| **Remover** | Removes marked infected fruits | Hybrid | Plans optimal path to targets with reactive obstacle avoidance |
| **Harvester** | Harvests mature fruits | Reactive | Detects and removes ready-to-harvest fruits |

### Agent Interactions

| Agent A | Agent B | Interaction Type | Description |
|---------|---------|------------------|-------------|
| Analyzer | Remover | Communication | Sends infected fruit coordinates for removal |
| Analyzer | Harvester | Coordination | Shares environment updates to avoid interference |
| Remover | Harvester | Cooperation | Collaboratively maintains crop quality and efficiency |

## 🛠️ Technologies Used

- **Unity 3D**: Simulation environment and agent modeling
- **C#**: Agent behavior and system logic
- **Git/GitHub**: Version control and collaboration
- **AI & Vision Techniques**: Anomaly detection algorithms

## 👥 Team Members

- **Jimena Díaz Franco** - Remover Agent Implementation, Repository Management
- **Ilan Gómez Guerrero** - Simulation Environment, Analyzer Agent
- **Maria Guadalupe Soto Acosta** - Harvester Agent, Integration & Testing
- **Francisco Raziel Andalón Aguayo** - Documentation, Communication
