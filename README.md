# API Monetization Gateway

A programmable API monetization gateway that sits between external API consumers and internal services, providing authentication, rate limiting, usage tracking, and billing coordination.

## Features

- **Rate Limiting**: Per-tier rate limiting and monthly quotas
- **Usage Tracking**: Comprehensive API usage logging
- **Billing Integration**: Automated monthly billing calculations

## Prerequisites

- .NET 8.0 SDK
- Docker and Docker Compose
- SQL Server 2022 (or use Docker version)
## ERD
\Doc\MonetizationDB ERD.png
## Quick Start

### Using Docker Compose (Recommended)

1. Clone the repository:
```bash
git clone https://github.com/tpics/MonetizationAPI.git
cd MonetizationAPI
