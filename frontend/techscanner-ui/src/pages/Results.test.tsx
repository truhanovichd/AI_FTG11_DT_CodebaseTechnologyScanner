import { describe, it, expect, beforeEach, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import Results from "../pages/Results";

describe("Results Component", () => {
  beforeEach(() => {
    // Mock sessionStorage
    const mockResults = {
      startedAt: new Date().toISOString(),
      filesScanned: 25,
      items: [
        { kind: "CSharp", name: "Backend", evidence: "Backend.csproj" },
        { kind: "Node.js", name: "Frontend", evidence: "package.json" },
        { kind: "Docker", name: "Container", evidence: "Dockerfile" },
      ],
    };
    sessionStorage.getItem = vi.fn(() => JSON.stringify(mockResults));
  });

  it("renders results page heading", () => {
    render(
      <BrowserRouter>
        <Results />
      </BrowserRouter>
    );

    const heading = screen.queryByText(/results|scan results/i);
    expect(heading || screen.getByText(/files scanned|detected/i)).toBeInTheDocument();
  });

  it("displays files scanned count", () => {
    render(
      <BrowserRouter>
        <Results />
      </BrowserRouter>
    );

    const fileCount = screen.queryByText(/25|scanned/i);
    expect(fileCount).toBeInTheDocument();
  });

  it("displays detected items", () => {
    render(
      <BrowserRouter>
        <Results />
      </BrowserRouter>
    );

    // Check for at least one detected item type
    const csharp = screen.queryByText(/CSharp|Backend/i);
    const nodejs = screen.queryByText(/Node.js|Frontend/i);
    const docker = screen.queryByText(/Docker|Container/i);

    expect(csharp || nodejs || docker).toBeInTheDocument();
  });

  it("renders navigation buttons", () => {
    render(
      <BrowserRouter>
        <Results />
      </BrowserRouter>
    );

    // Check for action buttons
    const buttons = screen.queryAllByRole("button");
    expect(buttons.length).toBeGreaterThan(0);
  });

  it("handles empty results gracefully", () => {
    sessionStorage.getItem = vi.fn(() =>
      JSON.stringify({
        startedAt: new Date().toISOString(),
        filesScanned: 0,
        items: [],
      })
    );

    render(
      <BrowserRouter>
        <Results />
      </BrowserRouter>
    );

    // Should not crash and display something
    expect(screen.queryByText(/results|files scanned|detected|empty/i)).toBeInTheDocument();
  });
});
