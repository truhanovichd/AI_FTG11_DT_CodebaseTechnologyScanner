import { describe, it, expect, beforeEach, afterEach } from "vitest";
import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import { Results } from "../main/Results";

describe("Results Component", () => {
  beforeEach(() => {
    const mockResults = {
      totalFiles: 25,
      csProjFiles: ["Backend.csproj"],
      packageJsonFiles: ["package.json"],
      dockerfiles: ["Dockerfile"],
    };
    sessionStorage.setItem("scanResults", JSON.stringify(mockResults));
  });

  afterEach(() => {
    sessionStorage.clear();
  });

  it("displays files scanned count", async () => {
    render(
      <MemoryRouter>
        <Results />
      </MemoryRouter>
    );

    const fileCount = await screen.findByText("25");
    expect(fileCount).toBeInTheDocument();
  });

  it("displays detected items section", () => {
    render(
      <MemoryRouter>
        <Results />
      </MemoryRouter>
    );

    const heading = screen.getByRole("heading", { name: /scan results/i });
    expect(heading).toBeInTheDocument();
  });
});
