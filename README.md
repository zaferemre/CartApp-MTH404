# CartApp - Setup and Running Instructions

Welcome to **CartApp**, a project consisting of a backend powered by MongoDB and .NET, and a frontend built with a shopping cart feature. Follow the steps below to set up and run the project successfully. Made by us students at Istanbul Technical University.

---

## Cloning the Repository

1. Open a terminal or command prompt.
2. Clone the repository:
   ```bash
   git clone https://github.com/zaferemre/CartApp-MTH404.git
   ```
3. Navigate to the cloned directory:
   ```bash
   cd CartApp-MTH404
   ```
4. Initialize and update the submodule:
   ```bash
   git submodule update --init --recursive
   ```

---

## Setting Up MongoDB

1. **Install MongoDB Compass**  
   Download and install [MongoDB Compass](https://www.mongodb.com/products/compass), a GUI for MongoDB.

2. **Create a Database and Cluster**  
   - Open MongoDB Compass.
   - Create a new database named **`CartApp`**.
   - Within this database, create a cluster (collection) also named **`CartApp`**.

3. **Configure Database Connection**  
   - Navigate to the `appsettings.json` file in the backend directory.
   - Locate the `ConnectionStrings` section:
     ```json
     "ConnectionStrings": {
         "MongoDb": "mongodb://localhost:27017/CartApp"
     }
     ```
   - Replace `"mongodb://localhost:27017/CartApp"` with your own MongoDB connection string if it differs.

---

## Running the Project

### Backend (API Server)
1. Open a terminal or command prompt in the project’s root directory.
2. Start the backend server:
   ```bash
   dotnet run
   ```

### Frontend (Shopping Cart)
1. Navigate to the `shopping-cart` directory:
   ```bash
   cd CartAPP-MTH404/shopping-cart
   ```
2. Start the development server:
   ```bash
   npm run dev
   ```
3. Once the server is running, it will generate a localhost URL (e.g., `http://localhost:3000`). Open this link in your browser to use the application.

---

## Usage

1. Ensure both the backend and frontend are running.
2. Use the frontend link (`http://localhost:3000`) to access the application.
3. Interact with the app to test its features, such as adding items to the shopping cart.

---

## Notes

- Ensure your MongoDB service is running locally.
- If you encounter any issues with the connection string, double-check your MongoDB setup and adjust the `appsettings.json` file accordingly.
- For any issues with dependencies, use:
  - `npm install` inside the `shopping-cart` folder to install frontend dependencies.
  - `dotnet restore` inside the backend folder to restore .NET dependencies.

---

Enjoy exploring CartApp! 😊
