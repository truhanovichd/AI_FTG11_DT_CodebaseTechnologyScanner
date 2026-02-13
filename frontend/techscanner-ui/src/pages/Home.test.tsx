import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import Home from "../pages/Home";

describe("Home Component", () => {
  it("renders home page heading", () => {
    render(
      <BrowserRouter>
        <Home />
      </BrowserRouter>
    );

    const heading = screen.getByRole("heading");
    expect(heading).toBeInTheDocument();
  });

  it("renders welcome message", () => {
    render(
      <BrowserRouter>
        <Home />
      </BrowserRouter>
    );

    const text = screen.getByText(/welcome|technology|scanner/i);
    expect(text).toBeInTheDocument();
  });

  it("displays description text", () => {
    render(
      <BrowserRouter>
        <Home />
      </BrowserRouter>
    );

    const container = screen.getByText(/scan|codebase|detect/i);
    expect(container).toBeInTheDocument();
  });
});
