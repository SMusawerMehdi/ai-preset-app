# AI Preset App

A multi-agent AI application built with a modular architecture where specialized agents collaborate to design, build, test, and secure the system.

## Tech Stack

- **Frontend:** React / Next.js
- **Backend:** .NET (ASP.NET Core)
- **AI Layer:** Claude API (Anthropic)
- **Architecture:** Multi-agent orchestration

## Folder Structure

```
ai-preset-app/
├── docs/                  # Project documentation and specifications
│   └── PROJECT_SPEC.md
├── agents/                # Agent definitions and instructions
│   ├── orchestrator.md    # Central coordinator agent
│   ├── backend_agent.md   # Backend development agent
│   ├── frontend_agent.md  # Frontend development agent
│   ├── ai_agent.md        # AI/ML integration agent
│   ├── qa_agent.md        # Quality assurance agent
│   └── security_agent.md  # Security review agent
├── tasks/                 # Task tracking and current work
│   └── current_task.md
└── outputs/               # Generated artifacts and results
```

## Multi-Agent System

The project uses a multi-agent architecture where each agent has a distinct role:

| Agent | Responsibility |
|-------|---------------|
| **Orchestrator** | Coordinates all agents, manages task flow, and resolves conflicts |
| **Backend Agent** | Designs and implements APIs, services, and data models (.NET) |
| **Frontend Agent** | Builds UI components, pages, and client-side logic (React) |
| **AI Agent** | Integrates AI capabilities, prompt engineering, and model interactions |
| **QA Agent** | Writes tests, reviews quality, and validates acceptance criteria |
| **Security Agent** | Audits code for vulnerabilities, enforces security best practices |

### How It Works

1. A task is defined in `tasks/current_task.md`
2. The **Orchestrator** reads the task and delegates subtasks to specialized agents
3. Each agent works within its domain, producing code or documentation in `outputs/`
4. The **QA Agent** validates the work against the project spec
5. The **Security Agent** reviews for vulnerabilities before merging
6. The Orchestrator assembles the final result and updates task status

## Getting Started

```bash
git clone https://github.com/<your-username>/ai-preset-app.git
cd ai-preset-app
```

## License

MIT
