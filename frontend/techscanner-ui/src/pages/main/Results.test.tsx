import { describe, it, expect, beforeEach, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import { Results } from "../main/Results";

describe("Results Component", () => {
  beforeEach(() => {
    const mockResults = {
      totalFiles: 25,
      csProjFiles: ["Backend.csproj"],
      packageJsonFiles: ["package.json"],
      dockerfiles: ["Dockerfile"],
    };
    sessionStorage.getItem = vi.fn(() => JSON.stringify(mockResults));
  });

  it("displays files scanned count", () => {
    render(<Results />);

    const fileCount = screen.queryByText(/25|scanned/i);
    expect(fileCount).toBeTruthy();
  });

  it("displays detected items section", () => {
    render(<Results />);

    const content = screen.getByText(/results|detected|files/i);
    expect(content).toBeInTheDocument();
  });
});
