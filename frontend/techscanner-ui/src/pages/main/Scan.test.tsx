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
});
