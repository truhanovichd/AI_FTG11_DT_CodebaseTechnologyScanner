import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import { Scan } from "../main/Scan";

describe("Scan Component", () => {
  it("renders directory path input", () => {
    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    const input = screen.getByLabelText(/directory path/i);
    expect(input).toBeInTheDocument();
  });

  it("renders scan button", () => {
    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    const button = screen.getByRole("button", { name: /scan/i });
    expect(button).toBeInTheDocument();
  });
});
