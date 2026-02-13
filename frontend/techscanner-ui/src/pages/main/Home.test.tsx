import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import { Home } from "../main/Home";

describe("Home Component", () => {
  it("renders heading", () => {
    render(<Home />);

    const heading = screen.getByRole("heading");
    expect(heading).toBeInTheDocument();
  });

  it("renders description text", () => {
    render(<Home />);

    const textMatches = screen.getAllByText(/scan|codebase|detect/i);
    expect(textMatches.length).toBeGreaterThan(0);
  });
});
