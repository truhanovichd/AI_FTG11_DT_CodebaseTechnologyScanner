import { describe, it, expect, beforeEach, afterEach, vi } from "vitest";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { BrowserRouter } from "react-router-dom";
import { Scan } from "../main/Scan";

describe("Scan Component", () => {
  beforeEach(() => {
    // Mock fetch globally
    global.fetch = vi.fn();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

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

  it("displays error message when API returns 400", async () => {
    // Mock failed API response
    (global.fetch as any).mockResolvedValueOnce({
      ok: false,
      status: 400,
      statusText: "Bad Request",
    });

    const user = userEvent.setup();

    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    // Enter directory path
    const input = screen.getByLabelText(/directory path/i);
    await user.type(input, "C:/invalid/path");

    // Click scan button
    const button = screen.getByRole("button", { name: /scan/i });
    await user.click(button);

    // Wait for error message to appear
    await waitFor(() => {
      const errorMessage = screen.getByText(/server error|bad request/i);
      expect(errorMessage).toBeInTheDocument();
    });
  });

  it("displays loading indicator while fetching", async () => {
    // Mock fetch with delayed promise (resolves after 100ms)
    (global.fetch as any).mockImplementation(
      () =>
        new Promise((resolve) =>
          setTimeout(
            () =>
              resolve({
                ok: true,
                json: async () => ({
                  csProjFiles: [],
                  packageJsonFiles: [],
                  dockerfiles: [],
                  totalFiles: 0,
                }),
              }),
            100
          )
        )
    );

    const user = userEvent.setup();

    render(
      <BrowserRouter>
        <Scan />
      </BrowserRouter>
    );

    // Enter directory path
    const input = screen.getByLabelText(/directory path/i);
    await user.type(input, "C:/projects");

    // Click scan button
    const button = screen.getByRole("button", { name: /scan|start/i });
    await user.click(button);

    // Assert loading indicator is visible
    const loadingText = screen.getByText(/scanning/i);
    expect(loadingText).toBeInTheDocument();

    // Wait for scan to complete (optional: verify it disappears)
    await waitFor(() => {
      expect(screen.queryByText(/scanning/i)).not.toBeInTheDocument();
    });
  });
});
