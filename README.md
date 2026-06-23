# AuroraBricks (BYU IS INTEX II Project)

This repository contains a past group project for the BYU Information Systems (IS) program. 

## 🌿 Branches

This repository is split into two primary branches:

* **`master`** (This branch): The original project codebase. It retains the production configurations and bindings used during development (which originally connected to cloud-hosted Azure SQL and Vault resources that are now offline).
* **`working-demo`**: A self-contained, fully operational version of the project configured for local runs. It utilizes:
  * Local SQLite databases instead of Azure SQL Server.
  * Automatic database seeding (products, categories, and recommendations).
  * Seeded local administrator and customer accounts.
  * Local HTTP configuration (no SSL dev certificates required).

---

## 🚀 How to Run the Demo

To run a fully working local version of the e-commerce store and dashboards, check out the **`working-demo`** branch and follow its detailed setup instructions:

```bash
git checkout working-demo
```