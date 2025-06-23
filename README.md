# 🧼 Fuzzy Logic Washing Machine Controller – C# WinForms

This project implements a **Fuzzy Inference System (FIS)** to simulate the intelligent control of a washing machine. It is developed in **C#** using **Windows Forms**, without any third-party fuzzy logic libraries. The system calculates the appropriate **detergent amount**, **washing time**, and **spin speed** based on fuzzy input variables such as **dirtiness**, **sensitivity**, and **laundry load**.

---

## 🎯 Objective

The goal is to simulate a real-world fuzzy controller for a washing machine by:
- Accepting input values from the user interface
- Performing fuzzification
- Applying **Mamdani inference rules**
- Defuzzifying outputs using **Weighted Average**
- Optionally also calculating **Centroid** method and visualizing rule activations

---

## 📥 Input Variables

| Variable     | Range     | Description                           |
|--------------|-----------|---------------------------------------|
| Dirtiness    | 0 - 10    | How dirty the laundry is              |
| Sensitivity  | 0 - 10    | Fabric delicacy level                 |
| Load Amount  | 0 - 10    | Laundry quantity in the drum          |

---

## 📤 Output Variables

| Output             | Description                           |
|--------------------|---------------------------------------|
| Detergent Amount   | Suggested detergent amount            |
| Washing Duration   | Duration of the washing cycle         |
| Spin Speed         | Rotation speed of the machine         |

---

## 🧠 System Components

- **Fuzzification**: Converts crisp input values into fuzzy values using trapezoidal or triangular membership functions
- **Rule Evaluation**: Uses **Mamdani inference** with manually defined rules (e.g., IF dirtiness is high AND load is heavy THEN detergent is much)
- **Defuzzification**:
  - **Weighted Average** (default method)
  - **Centroid of Area (optional, bonus)**
- **Rule Trigger Display**: All activated rules and their firing strengths are shown on screen
- **Graphics (optional)**: Plotting rule activation areas for visualization (bonus)

---

## 💻 Features

- Developed with **C# Windows Forms**
- Fully interactive form interface
- No external libraries used
- Clear object-oriented code structure
- Displays:
  - Fuzzified input values
  - Fired rules
  - Inferred fuzzy output sets
  - Final crisp output values
- Bonus:
  - Centroid defuzzification calculation
  - Graphical visualization of rule activation areas

---

## 🛠️ Technologies Used

- **Language:** C#
- **Framework:** .NET (Windows Forms)
- **Architecture:** Object-Oriented Programming (OOP)

---

## 🔧 How to Run

1. Clone the repository:
2. Open the solution in **Visual Studio**
3. Build and run the application
4. Enter values for dirtiness, sensitivity, and load
5. View the calculated outputs and activated rules

---

## 📁 Project Structure

- `FuzzySet.cs` – Contains membership functions
- `FuzzyRule.cs` – Defines fuzzy rules and rule base
- `FuzzyInferenceEngine.cs` – Implements Mamdani inference logic
- `Defuzzifier.cs` – Performs both Weighted Average and Centroid methods
- `MainForm.cs` – User interface logic

---

## 📊 Example

**Inputs:**
- Dirtiness: 8  
- Sensitivity: 3  
- Load Amount: 7  

**Outputs:**
- Detergent Amount: 80 ml  
- Washing Time: 42 min  
- Spin Speed: 900 RPM  

---

## 📌 Notes

- Fully implemented without using fuzzy logic libraries
- Modular and readable code
- Designed for educational purposes

---

## 🪪 License

MIT License

---

Ahmet Burak Akbaş
